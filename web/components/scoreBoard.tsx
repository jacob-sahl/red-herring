import React, { ReactComponentElement } from 'react'
import styles from "./scoreBoard.module.css"

interface ScoreBoardProps {
    visible: boolean
    scores: { id: number, score: number }[][]
    players: { id: number, name: string }[]
}


export default function ScoreBoard({ visible, scores, players }: ScoreBoardProps) {

    const getPlayerName = (id: number) => {
        const player = players.find(player => player.id === id);
        return player ? player.name : 'Unknown';
    }


    if (!visible) {
        return <></>
    }
    return (
        <div className={styles["scoreboard"]}>
            <div className={styles["title"]}>
                Player Name
            </div>

            <div className={styles["rounds"]}>
                <span>1</span>
                <span>2</span>
                <span>3</span>
                <span>4</span>
            </div>

            <div className={styles["total"]}>
                T
            </div>

            <div className={styles["player"]}>
                <div className={styles["player__name"]}>
                Player 2
                </div>

                <div className={styles["player__score"]}>20</div>
                <div className={styles["player__score"]}>22</div>
                <div className={styles["player__score"]}>27</div>
                <div className={styles["player__score"]}>24</div>

                <div className={styles["player__total"]}>
                    93
                </div>
            </div>

            <div className={styles["player"]}>
                <div className={styles["player__name"]}>
                    Player 2
                </div>

                <div className={styles["player__score"]}>20</div>
                <div className={styles["player__score"]}>26</div>
                <div className={styles["player__score"]}>29</div>
                <div className={styles["player__score"]}>25</div>

                <div className={styles["player__total"]}>
                    100
                </div>
            </div>
        </div>
    )
}
