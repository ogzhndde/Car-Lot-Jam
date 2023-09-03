using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// The class that produces all cars that can be produced.
/// </summary>
namespace CarFactoryStatic
{
    public static class CarFactory
    {
        //It stores all cars types and production classes in a dictionary.
        private static Dictionary<CarType, Func<CarProperties>> carFactories = new Dictionary<CarType, Func<CarProperties>>
        {
            { CarType.Sedan, () => new Sedan() },
            { CarType.Limousine, () => new Limousine() }
        };

        //The class in which cars are spawned.
        public static CarProperties SpawnCar(CarType carType, TeamType teamType, Vector3 spawnPosition, Quaternion spawnRotation, ref GameManager manager)
        {
            if (carFactories.TryGetValue(carType, out var factory))
            {
                var car = factory.Invoke();
                car.SpawnCar(carType, teamType, spawnPosition, spawnRotation, ref manager);
                return car;
            }
            else
            {
                return null;
            }
        }
    }

    //All necessary data for cars is drawn from scriptable objects and sent to the factory for production.
    public class Sedan : CarProperties
    {

        public override void SpawnCar(CarType carType, TeamType teamType, Vector3 spawnPosition, Quaternion spawnRotation, ref GameManager manager)
        {
            var spawnedCar = ObjectPoolManager.SpawnObjects(manager.SO.CarData.Sedan, spawnPosition, spawnRotation);

            manager.LevelCarCounter++;
            //For easy placement, cars are keeping in a parent. Taking them out of parent
            var childCar = spawnedCar.transform.GetChild(0);
            childCar.SetParent(null);
            childCar.gameObject.GetComponent<CarController>().SetCarProperties(carType, teamType);
        }
    }

    public class Limousine : CarProperties
    {
        public override void SpawnCar(CarType carType, TeamType teamType, Vector3 spawnPosition, Quaternion spawnRotation, ref GameManager manager)
        {
            var spawnedCar = ObjectPoolManager.SpawnObjects(manager.SO.CarData.Limousine, spawnPosition, spawnRotation);
            manager.LevelCarCounter++;

            //For easy placement, cars are keeping in a parent. Taking them out of parent
            var childCar = spawnedCar.transform.GetChild(0);
            childCar.SetParent(null);
            childCar.gameObject.GetComponent<CarController>().SetCarProperties(carType, teamType);
        }
    }
}
