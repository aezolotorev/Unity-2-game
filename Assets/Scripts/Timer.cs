using System;

public sealed class Timer
{
    private DateTime _start;
    private float  _elapsed=-1;
    TimeSpan _duration;
    public void Start(float elapsed)
    {
        _elapsed = elapsed;
        _start = DateTime.Now;
        _duration = TimeSpan.Zero;

    }
    public void Update()
    {
        if (_elapsed > 0)
        {
            _duration = DateTime.Now - _start;
            if (_duration.TotalSeconds > _elapsed)
            {
                _elapsed = 0;
            }
            if (_elapsed == 0)
            {
                _elapsed = -1;
            }
        }
    }
    public bool  IsEvent()
    {
        return _elapsed==0;
    }
 
}
