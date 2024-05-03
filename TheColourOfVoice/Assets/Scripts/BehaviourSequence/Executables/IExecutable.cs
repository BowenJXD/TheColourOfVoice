using System;
using System.Collections;
using System.Collections.Generic;

public interface IExecutable
{
    public IEnumerator Execute(Blackboard newExecutor);
}