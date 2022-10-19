import React from "react";
import window from "../assets/images/cards/foreground/window.png";
import bg1 from "../assets/images/cards/background/1.png";
import "./Card.css";
import { useSearchParams } from "react-router-dom";

interface CardInfo {
    b: string; // background
    f: string; // foreground
    c: string; // clue
    o: string; // objective
}

function Card() {
    const [searchParams] = useSearchParams();
    const requestPayloadBase64 = searchParams.get("r");
    if (!requestPayloadBase64) {
        return <div> No request payload </div>;
    }
    let cardInfo: CardInfo;
    try {
        cardInfo = JSON.parse(atob(requestPayloadBase64)) as CardInfo;
    } catch (error) {
        console.error(error);
        return <div> Invalid request payload </div>;
    }

    return (
        <div className="card-wrapper m-auto aspect-[10/16]">
            <div
                className="card m-auto cursor-pointer select-none hover:drop-shadow-2xl transition-shadow"
                style={{ backgroundImage: `url(${getBackground(cardInfo)})` }}
            >
                <div className="card-content card-clue ">
                    <p>Clue:</p>
                    <p>{cardInfo.c}</p>
                </div>
                <img
                    className="card-foreground"
                    src={getForeground(cardInfo)}
                />
                <div className="card-content card-objective">
                    <p>Secret Objective:</p>
                    <p>{cardInfo.o}</p>
                </div>
            </div>
        </div>
    );
}

const getBackground = (cardInfo: CardInfo) => {
    switch (cardInfo.b) {
        case "1":
            return bg1;
        default:
            return bg1;
    }
};

const getForeground = (cardInfo: CardInfo) => {
    switch (cardInfo.f) {
        case "window":
            return window;
        default:
            return window;
    }
};

export default Card;
