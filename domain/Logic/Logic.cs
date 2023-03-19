using System;

namespace Domain.Logic
{
    class IdCreator
    {
        private static IdCreator instance = null;
        private Random rand;

        private IdCreator()
        {
            DateTime epoch = DateTime.UnixEpoch;
            DateTime now = DateTime.UtcNow;

            TimeSpan ts = now.Subtract(epoch);
            double ms = ts.TotalMilliseconds;

            rand = new Random(Convert.ToInt32(ms));
        }

        public int NextId() {
            return rand.Next();
        }

        public static IdCreator getInstance() {
            if (instance == null)
                instance = new IdCreator();
            return instance;
        }
    }
}