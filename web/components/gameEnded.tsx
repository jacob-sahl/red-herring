import React, { ReactComponentElement } from 'react'
import { CurrentGameState } from '../firebase/modals/round'
import styles from "./scoreBoard.module.css"

interface GameEndedProps {
    roundInfo: CurrentGameState
}


export default function GameEnded({ roundInfo }: GameEndedProps) {
    return (
        <div className={styles["game-not-start__title"]}>
            Game Ended
        </div>
    )
}
