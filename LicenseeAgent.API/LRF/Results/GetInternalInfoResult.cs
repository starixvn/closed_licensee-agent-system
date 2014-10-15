namespace LicenseeAgent.API.LRF.Results
{
    using LicenseeAgent.API.LRF.Entities;

    /// <summary>
    /// GetInternalInfoResult class.
    /// </summary>
    public class GetInternalInfoResult
    {
        public int ErrCode { get; set; }

        public InternalInfo InternalInfo { get; set; }
    }
}