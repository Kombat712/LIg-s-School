import React, { useState, useEffect } from "react";
import { NavLink } from "react-router-dom";
import "./../assets/style/style_skills_tree.css";
import ProtectedRoute from "../hooks/ProtectedRoute";
import { useAuth } from "../hooks/AuthContext";
import QuizDayStats from "./../hooks/QuizDayStats";
import comu from "./../assets/image/comun.png"
import dat from "./../assets/image/dati.png"
import doc from "./../assets/image/documen.png"
import zadac from "./../assets/image/zadachi.png"
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

const OrgTree = () => {
    const [tests, setTests] = useState([]);
    const { isAuthenticated } = useAuth();
    const { availableOrgTests, totalOrgTests } = QuizDayStats();

    useEffect(() => {
        if (isAuthenticated) {
            fetchTestsInfo();
        }
    }, [isAuthenticated]);

    const fetchTestsInfo = async () => {
        try {
            const response = await fetch(`${API_BASE_URL}/orgtest`, {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`,
                },
            });
            if (response.ok) {
                const data = await response.json();
                setTests(data);
            }
        } catch (error) {
            console.error("Ошибка загрузки информации о тестах", error);
        }
    };

    const getRingColor = (testName) => {
        const t = tests.find((x) => x.name === testName);
        if (!t) return "#e0e0e0";
        return t.isCompleted ? "#27ae60" : "#e0e0e0";
    };

    const handleTestClick = (e) => {
        if (availableOrgTests === 0) {
            e.preventDefault();
            alert("Лимит попыток на сегодня исчерпан.");
        }
    };

    return (
        <ProtectedRoute>
            <div className="container_all">
                <div className="head">
                    <p>Навыки организатора
                        <br />
                        <span className="quiz-counter_new">
                            {availableOrgTests <= 0
                                ? "На сегодня все доступы исчерпаны"
                                : `Осталось тестов: ${availableOrgTests}/${totalOrgTests}`}
                        </span>
                    </p>
                </div>

                <div className="container_first">
                    <NavLink to="/orgtest/Test 1" className="skil-cont" onClick={handleTestClick}>
                        <div className="cir-cont" style={{ border: `5px solid ${getRingColor("Test 1")}` }}>
                            <img src={comu} alt="Test 1" />
                        </div>
                        <p>Тест 1</p>
                    </NavLink>
                </div>

                <div className="container_second">
                    <NavLink to="/orgtest/Test 2" className="skil-cont" onClick={handleTestClick}>
                        <div className="cir-cont" style={{ border: `5px solid ${getRingColor("Test 2")}` }}>
                            <img src={dat} alt="Test 2" />
                        </div>
                        <p>Тест 2</p>
                    </NavLink>
                    <NavLink to="/orgtest/Test 3" className="skil-cont" onClick={handleTestClick}>
                        <div className="cir-cont" style={{ border: `5px solid ${getRingColor("Test 3")}` }}>
                            <img src={doc} alt="Test 3" />
                        </div>
                        <p>Тест 3</p>
                    </NavLink>
                </div>

                <div className="container_last">
                    <NavLink to="/orgtest/Test 4" className="skil-cont" onClick={handleTestClick}>
                        <div className="cir-cont" style={{ border: `5px solid ${getRingColor("Test 4")}` }}>
                            <img src={zadac} alt="Test 4" />
                        </div>
                        <p>Тест 4</p>
                    </NavLink>
                </div>
            </div>
        </ProtectedRoute>
    );
};

export default OrgTree;
