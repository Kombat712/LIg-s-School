import { useState, useEffect, useCallback } from "react";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const QuizDayStats = () => {
    const [availableQuizzes, setAvailableQuizzes] = useState(0);
    const [totalQuizzes, setTotalQuizzes] = useState(3);
    const [availableOrgTests, setAvailableOrgTests] = useState(0);
    const [totalOrgTests, setTotalOrgTests] = useState(3);

    const fetchUserData = useCallback(async () => {
        try {
            const response = await fetch(`${API_BASE_URL}/profile/quiz-stats`, {
                method: "GET",
                headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
                credentials: "include",
            });

            if (!response.ok) throw new Error("Ошибка загрузки данных");

            const data = await response.json();
            // Бэкенд возвращает { attemptsLeft, totalLimit } — общий пул для квизов и тестов
            setAvailableQuizzes(data.attemptsLeft ?? 0);
            setTotalQuizzes(data.totalLimit ?? 3);
            setAvailableOrgTests(data.attemptsLeft ?? 0);
            setTotalOrgTests(data.totalLimit ?? 3);
        } catch (error) {
            console.error("Ошибка получения данных пользователя:", error);
        }
    }, []);

    useEffect(() => {
        fetchUserData();
    }, [fetchUserData]);

    return { 
        availableQuizzes, totalQuizzes, 
        availableOrgTests, totalOrgTests, 
        refreshStats: fetchUserData 
    };
};

export default QuizDayStats;
