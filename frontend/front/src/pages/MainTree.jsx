import React, { useState, useEffect } from "react";
import { NavLink } from "react-router-dom";
import ProtectedRoute from "../hooks/ProtectedRoute";
import "./../assets/style/style_skills_tree.css";
import chgk from "./../assets/image/svoyak50.png"
import org from "./../assets/image/predposilki.png"
const MainTree = () => {
  return (
    <ProtectedRoute>
      <div className="container_all" style={{ marginTop: '10%' }}>
        <div className="head">
          <p>Выберите направление</p>
        </div>

        <div className="container_second" style={{ gap: '20%' }}>
          <NavLink to="/skills" className="skil-cont">
            <img src={chgk} alt="ЧГК" />
            <p>ЧГК Игрок</p>
          </NavLink>

          <NavLink to="/orgtree" className="skil-cont">
            <img src={org} alt="ОРГ" />
            <p>Организатор игр</p>
          </NavLink>
        </div>
      </div>
    </ProtectedRoute>
  );
};

export default MainTree;
