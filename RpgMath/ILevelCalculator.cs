namespace RpgMath
{
    public interface ILevelCalculator
    {
        long ComputeHpGain(long level, int rank, long current);
        long ComputeLuckGain(long level, int rank, long current);
        long ComputeMpGain(long level, int rank, long current);
        long ComputePrimaryStatGain(long level, int rank, long current);
    }
}