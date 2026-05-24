import React, { useState, useEffect } from "react";
import { NavLink } from "react-router-dom";
import ProtectedRoute from "../hooks/ProtectedRoute";
import "./../assets/style/style_skills_tree.css";

const MainTree = () => {
  return (
    <ProtectedRoute>
      <div className="container_all" style={{ marginTop: '10%' }}>
        <div className="head">
          <p>Выберите направление</p>
        </div>

        <div className="container_second" style={{ gap: '20%' }}>
          <NavLink to="/skills" className="skil-cont">
            <img src="/tree/target.png" alt="ЧГК" />
            <p>ЧГК Игрок</p>
          </NavLink>

          <NavLink to="/orgtree" className="skil-cont">
            <img src="/tree/target.png" alt="ОРГ" />
            <p>Организатор игр</p>
          </NavLink>
        </div>
      </div>
    </ProtectedRoute>
  );
};

export default MainTree;
