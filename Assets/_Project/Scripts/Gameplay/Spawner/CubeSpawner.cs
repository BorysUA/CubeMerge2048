using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Gameplay.Cube;
using _Project.Scripts.Gameplay.Factory;
using _Project.Scripts.Gameplay.Repository;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Spawner
{
    public class CubeSpawner
    {
        private readonly ICubeFactory _cubeFactory;
        private readonly ICubeRepository _cubeRepository;

        private int _cubeCount;

        public CubeSpawner(ICubeFactory cubeFactory, ICubeRepository cubeRepository)
        {
            _cubeFactory = cubeFactory;
            _cubeRepository = cubeRepository;
        }

        public CubeActor Spawn(Vector3 position, IReadOnlyList<ValueChance> weightedValueChance)
        {
            _cubeCount++;
            CubeActor cubeActor = _cubeFactory.CreateCube(position);

            int value = WeightedRandom(weightedValueChance);
            cubeActor.Initialize(_cubeCount, value);
            _cubeRepository.Register(cubeActor);

            return cubeActor;
        }

        private int WeightedRandom(IReadOnlyList<ValueChance> pool)
        {
            int totalWeight = pool.Sum(x => x.Weight);
            int roll = Random.Range(0, totalWeight);
            int accumulator = 0;

            foreach (ValueChance valueChance in pool)
            {
                accumulator += valueChance.Weight;

                if (roll < accumulator)
                    return valueChance.Value;
            }

            return pool.Last().Value;
        }
    }
}