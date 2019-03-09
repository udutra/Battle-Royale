using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAnima
{
    void AnimaBool(string nome, bool gatilho);
    void AnimaFloat(string nome, float gatilho, float dampTime, float deltaTime);
}