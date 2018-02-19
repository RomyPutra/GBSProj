using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BK_TRANSFEES_Info
{
    private string _transID;
    private string _recordLocator = String.Empty;
    private int _seqNo;
    private string _feeCode;
    private string _feeDesc;
    private string _paxType;
    private string _origin;
    private string _transit;
    private string _destination;
    private int _feeType;
    private decimal _feeQty;
    private decimal _feeRate;
    private decimal _feeAmt;
    private byte _transvoid;
    private Guid _rowguid = Guid.Empty;
    private string _createBy;
    private DateTime _syncCreate;
    private DateTime _syncLastUpd;
    private string _lastSyncBy;

    #region Public Properties
    public string TransID
    {
        get { return _transID; }
        set { _transID = value; }
    }
    public string RecordLocator
    {
        get { return _recordLocator; }
        set { _recordLocator = value; }
    }
    public int SeqNo
    {
        get { return _seqNo; }
        set { _seqNo = value; }
    }
    public string FeeCode
    {
        get { return _feeCode; }
        set { _feeCode = value; }
    }
    public string FeeDesc
    {
        get { return _feeDesc; }
        set { _feeDesc = value; }
    }
    public string PaxType
    {
        get { return _paxType; }
        set { _paxType = value; }
    }
    public string Origin
    {
        get { return _origin; }
        set { _origin = value; }
    }
    public string Transit
    {
        get { return _transit; }
        set { _transit = value; }
    }
    public string Destination
    {
        get { return _destination; }
        set { _destination = value; }
    }
    public int FeeType
    {
        get { return _feeType; }
        set { _feeType = value; }
    }
    public decimal FeeQty
    {
        get { return _feeQty; }
        set { _feeQty = value; }
    }
    public decimal FeeRate
    {
        get { return _feeRate; }
        set { _feeRate = value; }
    }
    public decimal FeeAmt
    {
        get { return _feeAmt; }
        set { _feeAmt = value; }
    }

    public byte Transvoid
    {
        get { return _transvoid; }
        set { _transvoid = value; }
    }

    public Guid rowguid
    {
        get { return _rowguid; }
        set { _rowguid = value; }
    }

    public string CreateBy
    {
        get { return _createBy; }
        set { _createBy = value; }
    }
    public DateTime SyncCreate
    {
        get { return _syncCreate; }
        set { _syncCreate = value; }
    }

    public DateTime SyncLastUpd
    {
        get { return _syncLastUpd; }
        set { _syncLastUpd = value; }
    }
    public string LastSyncBy
    {
        get { return _lastSyncBy; }
        set { _lastSyncBy = value; }
    }
    #endregion

}

