import React from "react";
import { useSearchParams } from "react-router-dom";

function Card() {
    const [searchParams, setSearchParams] = useSearchParams();
    const requestPayload = searchParams.get("r");
    return <div>{requestPayload}</div>;
}

export default Card;
