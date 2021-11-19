//This file Contains the commonly used delegates in this game

public delegate void Event<T>(T arg1);
public delegate void Event<T1, T2>(T1 arg1, T2 arg2);
public delegate void Event<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
public delegate void Event<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
public delegate void Empty();