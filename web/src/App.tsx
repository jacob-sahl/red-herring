import React from "react";
import { Routes, Route, Link } from "react-router-dom";
import "./App.css";
import Card from "./components/Card";

function App() {
    return (
        <div className="App">
            <header className="App-header">
                <h1 className="text-3xl font-bold font-[XTypewriter]">
                    Red Herring!
                </h1>
            </header>
            <Routes>
                <Route path="/" element={<Card />} />
            </Routes>
        </div>
    );
}

export default App;
