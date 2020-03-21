using System.Collections.Generic;

public interface ISceneUpdatable
{
    void OnUpdate();
    void OnEnter();
    List<string> targetStateNames { get; }
    void OnInit(string stateName);
}

