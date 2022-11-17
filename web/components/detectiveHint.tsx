import React, { ReactComponentElement } from 'react'
import { CurrentGameState } from '../firebase/modals/round'
import styles from "./scoreBoard.module.css"

interface DectectiveHintProps {
    roundInfo: CurrentGameState
}


export default function DectectiveHint({ roundInfo }: DectectiveHintProps) {
    return (
        <div className={styles["game-not-start__title"]}>
            Dectective Hint
        </div>
    )
}
