import type { GetServerSideProps, InferGetServerSidePropsType, NextPage } from 'next'
import Head from 'next/head'
import NodeAppInstance from '../firebase/nodeApp';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { useEffect, useState } from 'react';
import { CurrentGameState } from '../firebase/modals/round';
import APIClient from '../utils/APIClient';
import Cookies from 'universal-cookie';
import ScoreBoard from '../components/scoreBoard';
import GameNotStart from '../components/gameNotStart';
import GameEnded from '../components/gameEnded';
import DectectiveHint from '../components/detectiveHint';
import InformantHint from '../components/informantHint';

const cookies = new Cookies();

const GamePage: NextPage<{ initialGameState: CurrentGameState }> = ({ initialGameState }) => {

    const [roundInfo, setRoundInfo] = useState(initialGameState);
    const [scoreBoardVisible, setScoreBoardVisible] = useState(false);


    if (roundInfo.currentRound <= 3) {
        useEffect(() => {
            toast.success(`Joined as ${initialGameState.players[initialGameState.playerId].name}`, { toastId: 'join' });
            const interval = setInterval(() => {
                APIClient.getInstance().post<CurrentGameState>(`/games/${roundInfo.gameId}`,
                    { gameId: cookies.get("gameId"), playerId: cookies.get("playerId"), session: cookies.get("session") }
                ).then((response) => {
                    setRoundInfo(response.data);
                }
                ).catch((error) => {
                    toast(error.response.data.error || error.response.data.message);
                }
                );
            }, 5000);
            return () => clearInterval(interval);
        }, []);
    }

    let body = <></>
    if (roundInfo.currentRound === -1) {
        body = <GameNotStart roundInfo={roundInfo} />
    } else if (roundInfo.currentRound > 3) {
        body = <GameEnded roundInfo={roundInfo} />
    } else {
        if (roundInfo.isDetective) {
            body = <DectectiveHint roundInfo={roundInfo} />
        } else {
            body = <InformantHint roundInfo={roundInfo} />
        }
    }

    return (
        <div className="flex min-h-screen flex-col items-center justify-center py-2">
            <Head>
                <title>Red Herring</title>
                <link rel="icon" href="/favicon.ico" />
            </Head>

            <main className="flex w-full flex-1 flex-col items-center justify-center px-20 text-center">

                <h1>
                    {JSON.stringify(roundInfo)}
                </h1>
                {body}
                {/* <button className='bg-red-900 text-white rounded-md p-2 m-4 uppercase shadow-md hover:shadow-lg focus:shadow-lg focus:outline-none focus:ring-0 active:shadow-lg transition duration-150 ease-in-out' onClick={() => setScoreBoardVisible(!scoreBoardVisible)}>Scoreboard</button> */}
                {/* <ScoreBoard visible={scoreBoardVisible} players={roundInfo.players} scores={roundInfo.scores} /> */}
                <ToastContainer
                    position="bottom-center"
                    autoClose={3000}
                    hideProgressBar={false}
                    newestOnTop={false}
                    closeOnClick
                    rtl={false}
                    draggable
                    pauseOnHover
                    theme="colored"
                />
            </main>
        </div>
    )
}

export default GamePage

export const getServerSideProps: GetServerSideProps = async (context) => {
    const { gameId, playerId, session } = context.req.cookies;
    if (gameId && playerId && session) {

        if (NodeAppInstance.validatePlayer(gameId, Number(playerId), session)) {
            return {
                props: {
                    initialGameState: await NodeAppInstance.getPlayerRoundInfo(gameId, Number(playerId))
                }
            }
        }
    }
    return {
        redirect: {
            destination: '/join',
            permanent: false,
        },
    }
}