namespace Registration.Components;

using System;


[Serializable]
public class TransientException :
    Exception
{
    public TransientException()
    {
    }

    public TransientException(string message)
        : base(message)
    {
    }
}