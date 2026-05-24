import React, { useState, useEffect, useRef } from "react";
import { useParams, useNavigate, NavLink } from "react-router-dom";
import ProtectedRoute from "../hooks/ProtectedRoute";
import { useAuth } from "../hooks/AuthContext";
import "./../assets/style/style_quez.css";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

const OrgTest = () => {
  const { testName } = useParams();
  const navigate = useNavigate();
  const [questions, setQuestions] = useState([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [selectedAnswers, setSelectedAnswers] = useState([]);
  const [feedback, setFeedback] = useState("");
  const [correctAnswersCount, setCorrectAnswersCount] = useState(0);
  const [canStartTest, setCanStartTest] = useState(null);
  const [isCompleted, setIsCompleted] = useState(false);
  const [testResult, setTestResult] = useState(null);
  const { isAuthenticated } = useAuth();
  
  const isFetched = useRef(false);
  const isChecked = useRef(false);

  useEffect(() => {
    if (isAuthenticated === false) {
      navigate("/auth");
      return;
    }
    if (!testName) {
      navigate("/skills");
      return;
    }
    if (!isChecked.current) {
      checkCanStartTest();
      isChecked.current = true;
    }
  }, [testName, navigate, isAuthenticated]);

  useEffect(() => {
    if (canStartTest && !isFetched.current) {
      fetchQuestions();
      isFetched.current = true;
    }
  }, [canStartTest]);

  const checkCanStartTest = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}/orgtest/can-start`, {
        method: "GET",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        credentials: "include",
      });
      const data = await response.json();
      if (data.canStart) {
        setCanStartTest(true);
      } else {
        alert(data.message || "Вы не можете начать тест");
        navigate("/skills");
      }
    } catch (error) {
      console.error("Ошибка при проверке возможности начала теста:", error);
    }
  };

  const fetchQuestions = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}/orgtest/${testName}`, {
        method: "GET",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        credentials: "include",
      });
      if (!response.ok) {
        throw new Error(`Тест "${testName}" не найден`);
      }
      const data = await response.json();
      setQuestions(data);
    } catch (error) {
      console.error("Ошибка загрузки теста:", error);
    }
  };

  const toggleAnswer = (answerId) => {
    if (feedback) return; // Не даем менять после ответа
    setSelectedAnswers(prev => 
      prev.includes(answerId) 
        ? prev.filter(id => id !== answerId)
        : [...prev, answerId]
    );
  };

  const handleAnswerSubmit = async () => {
    if (selectedAnswers.length === 0) {
      setFeedback("Пожалуйста, выберите хотя бы один ответ.");
      return;
    }

    try {
      const response = await fetch(`${API_BASE_URL}/orgtest/check-answer`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
        body: JSON.stringify({
          questionId: questions[currentQuestionIndex].id,
          answerIds: selectedAnswers
        }),
      });
      
      const data = await response.json();
      
      if (data.isCorrect) {
        setFeedback(`Правильно! ${data.explanation || ""}`);
        setCorrectAnswersCount(prev => prev + 1);
      } else {
        setFeedback("Неправильно.");
      }
    } catch (error) {
      console.error("Ошибка при проверке ответа", error);
    }
  };

  const handleNextQuestion = () => {
    if (currentQuestionIndex < questions.length - 1) {
      setCurrentQuestionIndex(prev => prev + 1);
      setSelectedAnswers([]);
      setFeedback("");
    } else {
      completeTest(correctAnswersCount);
    }
  };

  const completeTest = async (finalCorrectAnswersCount) => {
    try {
      const response = await fetch(`${API_BASE_URL}/orgtest/complete-test`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
        body: JSON.stringify({ 
          testName, 
          correctAnswersCount: finalCorrectAnswersCount 
        }),
      });

      const result = await response.json();
      setIsCompleted(true);
      setTestResult(result);
    } catch (error) {
      console.error("Ошибка завершения теста:", error);
    }
  };

  if (canStartTest === null) return <p>Проверка...</p>;
  if (!questions.length) return <p>Загрузка вопросов...</p>;

  const currentQuestion = questions[currentQuestionIndex];

  return (
    <ProtectedRoute>
      <div className="quiz-popup">
        {!testResult ? (
          <>
            <div className="quiz-header">
              <p>Вопрос {currentQuestionIndex + 1} из {questions.length}</p>
            </div>
            <div className="quiz-body">
              <div className="quiz-name">{testName}</div>
              <p className="quiz-question-text">{currentQuestion.description}</p>
              
              <div className="answers-list" style={{display: 'flex', flexDirection: 'column', gap: '10px', margin: '20px 0'}}>
                {currentQuestion.answers.map(answer => (
                  <button 
                    key={answer.id}
                    className={`answer-btn ${selectedAnswers.includes(answer.id) ? 'selected' : ''}`}
                    onClick={() => toggleAnswer(answer.id)}
                    style={{
                      padding: '10px', 
                      backgroundColor: selectedAnswers.includes(answer.id) ? '#f39c12' : '#2b1b54',
                      color: 'white',
                      border: '2px solid #f39c12',
                      borderRadius: '5px',
                      cursor: feedback ? 'default' : 'pointer'
                    }}
                    disabled={!!feedback}
                  >
                    {answer.text}
                  </button>
                ))}
              </div>

              <button onClick={feedback ? handleNextQuestion : handleAnswerSubmit}>
                {feedback ? (currentQuestionIndex < questions.length - 1 ? "Следующий вопрос" : "Завершить тест") : "Ответить"}
              </button>
              
              <p className={`quiz-feedback ${feedback.includes('Правильно') ? 'correct' : 'wrong'}`}>
                {feedback}
              </p>
            </div>
            <div className="quiz-footer">
              <NavLink to="/skills" className="back-to-menu">
                <button>На дерево</button>
              </NavLink>
            </div>
          </>
        ) : (
          <>
            <div className="quiz-header">
              <h1>{testResult.name}</h1>
            </div>
            <div className="quiz-body">
              <h2 className={testResult.isPassed ? "result-success" : "result-fail"}>
                {testResult.isPassed ? "🎉 Тест пройден!" : "❌ Тест не пройден"}
              </h2>
              <p>🌟 Получено опыта: <strong>{testResult.totalXP}</strong></p>
              <p>💰 Получено монеток: <strong>{testResult.totalRings}</strong></p>
            </div>
            <div className="quiz-footer">
              <NavLink to="/skills" className="back-to-menu">
                <button>На дерево</button>
              </NavLink>
            </div>
          </>
        )}
      </div>
    </ProtectedRoute>
  );
};

export default OrgTest;
