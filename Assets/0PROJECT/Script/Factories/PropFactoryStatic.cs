using System;
using System.Collections.Generic;
using UnityEngine;

namespace PropFactoryStatic
{
    public static class PropFactory
    {
        //It stores all props types and production classes in a dictionary.
        private static Dictionary<PropType, Func<PropProperties>> propFactories = new Dictionary<PropType, Func<PropProperties>>
        {
            { PropType.RoadLinear, () => new RoadLinear() },
            { PropType.RoadT, () => new RoadT() },
            { PropType.RoadCorner, () => new RoadCorner() },
            { PropType.Grid, () => new Grid()}
        };

        //The class in which props are spawned.
        public static PropProperties SpawnProp(PropType propType, Vector3 spawnPosition, Quaternion spawnRotation, ref GameManager manager)
        {
            if (propFactories.TryGetValue(propType, out var factory))
            {
                var prop = factory.Invoke();
                prop.SpawnProp(propType, spawnPosition, spawnRotation, ref manager);
                return prop;
            }
            else
            {
                return null;
            }
        }
    }

    //All necessary data for props is drawn from scriptable objects and sent to the factory for production.
    public class RoadLinear : PropProperties
    {
        public override void SpawnProp(PropType propType, Vector3 spawnPosition, Quaternion spawnRotation, ref GameManager manager)
        {
            var spawnedProp = ObjectPoolManager.SpawnObjects(manager.SO.PropData.RoadLinear, spawnPosition, spawnRotation, PoolType.Gameobject);

            spawnedProp.name = spawnPosition.x.ToString() + "-" + spawnPosition.z.ToString();
        }
    } 
    
    //All necessary data for props is drawn from scriptable objects and sent to the factory for production.
    public class RoadT : PropProperties
    {
        public override void SpawnProp(PropType propType, Vector3 spawnPosition, Quaternion spawnRotation, ref GameManager manager)
        {

            var spawnedProp = ObjectPoolManager.SpawnObjects(manager.SO.PropData.RoadT, spawnPosition, spawnRotation, PoolType.Gameobject);
            spawnedProp.name = spawnPosition.x.ToString() + "-" + spawnPosition.z.ToString();
        }
    } 
    
    //All necessary data for props is drawn from scriptable objects and sent to the factory for production.
    public class RoadCorner : PropProperties
    {
        public override void SpawnProp(PropType propType, Vector3 spawnPosition, Quaternion spawnRotation, ref GameManager manager)
        {

            var spawnedProp = ObjectPoolManager.SpawnObjects(manager.SO.PropData.RoadCorner, spawnPosition, spawnRotation, PoolType.Gameobject);
            spawnedProp.name = spawnPosition.x.ToString() + "-" + spawnPosition.z.ToString();
        }
    } 
    
    //All necessary data for props is drawn from scriptable objects and sent to the factory for production.
    public class Grid : PropProperties
    {
        GameManager manager;

        public override void SpawnProp(PropType propType, Vector3 spawnPosition, Quaternion spawnRotation, ref GameManager manager)
        {
            var spawnedProp = ObjectPoolManager.SpawnObjects(manager.SO.PropData.Grid, spawnPosition, spawnRotation, PoolType.Gameobject);
        }
    }



}
