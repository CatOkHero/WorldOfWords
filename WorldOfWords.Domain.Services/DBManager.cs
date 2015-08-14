namespace WorldOfWords.Domain.Services
{
    public static class DBManager
    {
        private static WorldOfWordsUow _unitOfWork;
        public static WorldOfWordsUow UnitOfWork
        {
            get
            {
                return _unitOfWork ?? (_unitOfWork = new WorldOfWordsUow());
            }
        }
    }
}
