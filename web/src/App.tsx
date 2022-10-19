import React from "react";
import { Routes, Route } from "react-router-dom";
import "./App.css";
import bg from "./assets/images/bg.jpg";
import Card from "./components/Card";

function App() {
    return (
        <div className="App">
            <div
                className="App-background"
                // style={{ backgroundImage: `url(${bg})` }}
            ></div>
            <header className="App-header">
                <h1 className="text-3xl font-bold font-[XTypewriter] text-white select-none">
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
