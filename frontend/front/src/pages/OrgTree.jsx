import React from "react";
import { NavLink } from "react-router-dom";
import "./../assets/style/style_skills_tree.css";
import ProtectedRoute from "../hooks/ProtectedRoute";

const OrgTree = () => {
    return (
        <ProtectedRoute>
            <div className="container_all">
                <div className="head">
                    <p>Навыки организатора</p>
                </div>

                <NavLink to="/orgtest/Test 1" className="container_first">
                    <div className="skil-cont">
                        <img src="/tree/target.png" alt="Test 1" />
                        <p>Тест 1</p>
                    </div>
                </NavLink>

                <div className="container_second">
                    <NavLink to="/orgtest/Test 2" className="skil-cont">
                        <img src="/tree/target.png" alt="Test 2" />
                        <p>Тест 2</p>
                    </NavLink>
                    <NavLink to="/orgtest/Test 3" className="skil-cont">
                        <img src="/tree/target.png" alt="Test 3" />
                        <p>Тест 3</p>
                    </NavLink>
                </div>

                <div className="container_last">
                    <NavLink to="/orgtest/Test 4" className="skil-cont">
                        <img src="/tree/target.png" alt="Test 4" />
                        <p>Тест 4</p>
                    </NavLink>
                </div>
            </div>
        </ProtectedRoute>
    );
};

export default OrgTree;
