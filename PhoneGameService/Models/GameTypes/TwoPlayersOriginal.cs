﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoneGameService.Repositories;
using PhoneGameService.Models.GameStates;
using PhoneGameService.Models.EdgeConditionals;
using PhoneGameService.Logging;

namespace PhoneGameService.Models
{
    public class TwoPlayersOriginal : GameType
    {
        internal TwoPlayersOriginal() { }


        public override int id
        {
            get { return 1; }
        }

        public override string name
        {
            get { return "Original"; }
        }

        public override string description
        {
            get { return "Two players try to guess what was said."; }
        }

        public override int maxNumberOfPlayers
        {
            get { return 2; }
        }

        public override int minNumberOfPlayers
        {
            get { return 2; }
        }

        internal override GameType Initialize()
        {
            var notStarted = AddNode<NotStarted>(1, "Not_Started");
            var pickPlayer2 = AddNode<PickPlayer>(1, "Pick_Player_2");
            var pickPhrase = AddNode<PickPhrase>(1, "Pick_Phrase" );
            var player1Ready1 = AddNode<PlayerReady>(1, "Wait_For_Player_1");
            var player1SpeakPhrase = AddNode<PlayerSpeakPhrase>(1, "Player_1_Playing");
            var notifyPlayer2 = AddNode<NotifyPlayer>(2, "Notify_Player_2");
            var player2Ready = AddNode<PlayerReady>(2, "Wait_For_Player_2");
            var listenSpeakAnswer = AddNode<PlayerListenSpeakAnswer>(2, "Player_2_Playing");
            var notifyPlayer1 = AddNode<NotifyPlayer>(1, "Notify_Player_1");
            var player1Ready2 = AddNode<PlayerReady>(1, "Wait_For_Player_1_Again");
            var listenAnswer = AddNode<PlayerListenAnswer>(1, "Player_1_Final_Play");
            var endGame = AddNode<EndGame>(0, "End_Game");

            AddEdge<NoCondition>(notStarted, pickPlayer2, "Start");
            AddEdge<CheckNumberOfPlayers>(pickPlayer2, pickPhrase, "Player 2 chosen");
            AddEdge<CheckPhraseChosen>(pickPhrase, player1Ready1, "Phrase chosen");
            AddEdge<NoCondition>(player1Ready1, player1SpeakPhrase, "Player1 ready");
            AddEdge<NoCondition>(player1Ready1, pickPhrase, "Pick a different phrase");
            AddEdge<NoCondition>(player1Ready1, pickPlayer2, "Pick a different player2");
            AddEdge<CheckPhoneCallError>(player1SpeakPhrase, notifyPlayer2, "No error");
            AddEdge<NoCondition>(player1SpeakPhrase, player1Ready1, "Error");
            AddEdge<NoCondition>(notifyPlayer2, player2Ready, "Player1 turn end");
            AddEdge<NoCondition>(player2Ready, listenSpeakAnswer, "Player2 ready");
            AddEdge<CheckPhoneCallError>(listenSpeakAnswer, notifyPlayer1, "No error");
            AddEdge<NoCondition>(listenSpeakAnswer, player2Ready, "Error");
            AddEdge<NoCondition>(notifyPlayer1, player1Ready2, "Player2 turn end");
            AddEdge<NoCondition>(player1Ready2, listenAnswer, "Player1 ready");
            AddEdge<CheckPhoneCallError>(listenAnswer, endGame, "No error");
            AddEdge<NoCondition>(listenAnswer, player1Ready2, "Error");

            startNode = notStarted;

            return this;
        }
    }
}
