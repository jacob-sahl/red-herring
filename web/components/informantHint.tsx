import React, { ReactComponentElement } from 'react'
import { CurrentGameState } from '../firebase/modals/round'
import styles from "./scoreBoard.module.css"

interface InformantHintProps {
    roundInfo: CurrentGameState
}


export default function InformantHint({ roundInfo }: InformantHintProps) {
    return (
        <div className={styles["game-not-start__title"]}>
            Informant Hint
        </div>
    )
}
