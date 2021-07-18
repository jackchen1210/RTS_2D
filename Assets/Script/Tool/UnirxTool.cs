using System;
using UniRx;

public static  class UnirxTool
{

    public static IObservable<Unit> ToObserverable<T>(Action<Action<T>> action)
    {
        Subject<Unit> subject = new Subject<Unit>();
        action((type)=>
        subject.OnCompleted());
        return subject;
    }
}