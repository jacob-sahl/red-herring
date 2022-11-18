import type { GetServerSideProps, InferGetServerSidePropsType, NextPage } from 'next'
import Head from 'next/head'
import Image from 'next/image'
import { FormEvent, useState } from 'react'
import NodeAppInstance from '../firebase/nodeApp'
import axios from 'axios'
import APIClient from '../utils/APIClient'
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import Player from '../firebase/modals/player'
import Cookies from 'universal-cookie';

const cookies = new Cookies();

const JoinPage: NextPage<{joinCodeProp: string}> = ( {joinCodeProp } ) => {
  const [joinCode, setJoinCode] = useState(joinCodeProp);
  const [playerName, setPlayerName] = useState("");

  let handleJoin = async (e: FormEvent) => {
    e.preventDefault();
    let data = {
      'playerName': playerName,
      'joinCode': `${joinCode}`
    };
    APIClient.getInstance().post<Player>('/games/join', data).then((response) => {
      response.data && toast.success(`Joined as ${response.data.name}, redirecting...`);
      cookies.set('playerId', response.data.id, { path: '/' });
      cookies.set('session', response.data.session, { path: '/' });
      cookies.set('gameId', response.data.gameId, { path: '/' });

      window.location.href = `/`;
    }).catch((error) => {
      toast(error.response?.data?.error || error.response?.data?.message);
    });
  }

  return (
    <div className="flex min-h-screen flex-col items-center justify-center py-2">
      <Head>
        <title>Join Red Herring</title>
        <link rel="icon" href="/favicon.ico" />
      </Head>

      <main className="flex w-full flex-1 flex-col items-center justify-center px-20 text-center">
        <div className="block p-6 rounded-lg shadow-lg bg-white max-w-sm">
          <h2 className="text-3xl font-bold my-4 text-red-900">Join Red Herring</h2>
          <form onSubmit={handleJoin}>
            <input type="text" required className="text-center border-2 border-black rounded-md p-2 appearance-none m-2" placeholder="Enter your name" value={playerName} onChange={(e) => setPlayerName(e.target.value)} />
            <input type="number" required min={0} max={9999} className="text-center border-2 border-black rounded-md p-2 m-2 appearance-none" placeholder="Enter your code" value={joinCode} onChange={(e) => setJoinCode(e.target.value)} />
            <br />
            <button className="bg-red-900 text-white rounded-md p-2 m-4" type='submit'>Join</button>
          </form>
        </div>
        <ToastContainer
          position="bottom-center"
          autoClose={5000}
          hideProgressBar={false}
          newestOnTop={false}
          closeOnClick
          rtl={false}
          pauseOnFocusLoss
          draggable
          pauseOnHover
          theme="colored"
        />
      </main >
    </div >
  )
}

export default JoinPage

export const getServerSideProps : GetServerSideProps = async (context) => {
  const { joinCode } = context.query;
  return {
    props: {
      joinCodeProp: joinCode || ""
    }
  }
}