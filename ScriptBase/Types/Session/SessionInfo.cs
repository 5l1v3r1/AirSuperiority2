﻿using AirSuperiority.ScriptBase.Entities;
using GTA;

namespace AirSuperiority.ScriptBase.Types
{
    public struct SessionPlayer
    {
        public int TeamIdx { get; set; }

        public GameParticipant EntityRef { get; set; }

        public SessionPlayer(int teamIdx, GameParticipant entityRef)
        {
            TeamIdx = teamIdx;
            EntityRef = entityRef;
        }

        public void Update()
        {
            if (EntityRef != null)
            {
                EntityRef.OnUpdate(Game.GameTime);
            }
        }
    }

    public struct SessionInfo
    {
        public int LevelIndex { get; set; }
        public int NumPlayers { get; set; }
        public SessionPlayer[] Players { get; set; }

        public SessionPlayer AddPlayer(int playerIndex, int teamIdx, GameParticipant entityRef)
        {
            SessionPlayer player = new SessionPlayer(teamIdx, entityRef);
            Players[playerIndex] = player;
            return player;
        }

        public SessionPlayer AddPlayer(int playerIndex, int teamIdx)
        {
            return AddPlayer(playerIndex, teamIdx, null);
        }
    }
}
