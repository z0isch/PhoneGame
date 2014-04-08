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
    public class TestingGameType : GameType
    {
        internal TestingGameType() { }


        public override int id
        {
            get { return 2; }
        }

        public override string name
        {
            get { return "Testing Game Type"; }
        }

        public override string description
        {
            get { return "This game type is for unit testing only."; }
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
            var pickPhrase = AddNode<PickPhrase>(1, "Pick_Phrase");
            var testNode = AddNode<TestNode>(1, "Test_Node");
            var endGame = AddNode<EndGame>(0, "End_Game");

            AddEdge<NoCondition>(notStarted, pickPlayer2, "Pick player2");
            AddEdge<NoCondition>(notStarted, pickPhrase, "Pick phrase");
            AddEdge<CheckNumberOfPlayers>(pickPlayer2, testNode, "Player 2 chosen");
            AddEdge<CheckPhraseChosen>(pickPhrase, testNode, "Phrase chosen");
            AddEdge<CheckPhoneCallError>(testNode, endGame, "End game");
            AddEdge<CheckPhoneCallError>(testNode, notStarted, "Restart game");

            startNode = notStarted;

            return this;
        }


        public override GameStateNode GetNode(int id)
        {
            return nodes[id];
        }

        internal GameStateNode GetNextGameState(Game game, int currentNodeId, int edgeId)
        {
            try
            {
                GameStateNode node = nodes[currentNodeId];
                EdgeConditional edge = node.edgeConditionals.First<EdgeConditional>(e => e.id == edgeId);
                return edge.nextNode;
            }
            catch (Exception ex)
            {
                throw new PhoneGameClientException(string.Format("Could not get next GameStateNode.  Node id: {0} edge id: {1}", currentNodeId, edgeId), ex);
            }
        }

        internal bool TryGetNextGameState(Game game, int currentNodeId, int edgeId, out GameStateNode nextNode)
        {
            try
            {
                GameStateNode node = nodes[currentNodeId];
                EdgeConditional edge = node.edgeConditionals.First<EdgeConditional>(e => e.id == edgeId);
                nextNode = edge.nextNode;
                return true;
            }
            catch (Exception ex)
            {
                nextNode = null;
                return false;
            }
        }
    }
}
