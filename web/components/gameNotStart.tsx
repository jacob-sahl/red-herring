import React, { ReactComponentElement } from 'react'
import { CurrentGameState } from '../firebase/modals/round'
import styles from "./scoreBoard.module.css"

interface GameNotStart {
    roundInfo: CurrentGameState
}


export default function GameNotStart({ roundInfo }: GameNotStart) {
    return (
        <div>
            <p>Waiting for the game to start...</p>
            <p>Joined as {roundInfo.players[roundInfo.playerId].name}</p>

            <p>Joined Players:</p>
            {roundInfo.players.map((player, index) => {
                if (index !== roundInfo.playerId) {
                    return (
                        <p key={index}>
                            {player.name}
                        </p>
                    )
                } else {
                    return <></>
                }
            })}
        </div>
    )
}
