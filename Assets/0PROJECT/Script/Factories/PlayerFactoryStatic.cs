using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// The class that produces all players that can be produced.
/// </summary>

namespace PlayerFactoryStatic
{
    public static class PlayerFactory
    {
        //It stores all players types and production classes in a dictionary.
        private static Dictionary<PlayerType, Func<PlayerProperties>> playerFactories = new Dictionary<PlayerType, Func<PlayerProperties>>
        {
            { PlayerType.Normal, () => new NormalPlayer() }
        };

        //The class in which players are spawned.
        public static PlayerProperties SpawnPlayer(PlayerType playerType, TeamType teamType, Vector3 spawnPosition, ref GameManager manager)
        {
            if (playerFactories.TryGetValue(playerType, out var factory))
            {
                var player = factory.Invoke();
                player.SpawnPlayer(playerType, teamType, spawnPosition, ref manager);
                return player;
            }
            else
            {
                return null;
            }
        }
    }

    //All necessary data for players is drawn from scriptable objects and sent to the factory for production.
    public class NormalPlayer : PlayerProperties
    {
        public override void SpawnPlayer(PlayerType playerType, TeamType teamType, Vector3 spawnPosition, ref GameManager manager)
        {
            var spawnedPlayer = ObjectPoolManager.SpawnObjects(manager.SO.PlayerData.NormalPlayer, spawnPosition, Quaternion.identity);

            spawnedPlayer.GetComponent<PlayerController>().SetPlayerProperties(playerType, teamType);
        }
    }

}
