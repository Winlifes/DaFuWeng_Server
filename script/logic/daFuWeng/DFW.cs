public class DFW : IJudgment
{
    public bool Judge(int count)
    {
        if (count < 2 || count > 6) return false;
        else return true;
    }
}
