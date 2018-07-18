namespace olmelabs.battleship.api.Models
{
    public class GameOptions
    {
        /// <summary>
        /// Sleep time in ms for background player
        /// </summary>
        public int BackgroundPlayerDelayTime { get; set; } = 5000;

        /// <summary>
        /// Sleep time in ms for statistics collector
        /// </summary>
        public int BackgroundStatisticsCollectorDelayTime { get; set; } = 5000;

        public bool UseStatistics { get; set; }
    }
}
