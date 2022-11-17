import React, { ReactComponentElement } from 'react'
import { CurrentGameState } from '../firebase/modals/round'
import styles from "./scoreBoard.module.css"

interface GameNotStart {
    roundInfo: CurrentGameState
}


export default function GameNotStart({ roundInfo }: GameNotStart) {
    return (
        <div className={styles["game-not-start"]}>
            <div className={styles["game-not-start__title"]}>
                Game Not Started
            </div>
        </div>
    )
}
