using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABS.Logic.GroupBooking
{
    public class SeatInfo
    {
        #region "Private Members"

        private System.String _PassengerID;
        private System.String _SelectedSeat;
        private System.String _CurrentSeat;
        private System.String _PaxName;
        private System.String _PaxType;
        private System.String _FromTo;
        private System.Int32 _Seq;
        private string _CreatedDate;
        private string _CreatedBy;
        private string _LastAccess;
        private System.Byte _Status;
        private string _AccessDate;
        private System.Byte _Flag;
        private System.Int32 _PassengerNumber;
        private string _CompartmentDesignator;
        private string _Deck;
        private string _Orientation;
        private string _SeatSet;
        //added by ketee
        private System.Decimal _SeatAmount;
        private System.String _RecordLocator;
        private System.String _SeatGroup;

        private System.Int32 _IsHotSeat;
        #endregion

        #region "Public Properties"
        public System.String PassengerID
        {
            get { return _PassengerID; }
            set { _PassengerID = value; }
        }

        public System.String PaxName
        {
            get { return _PaxName; }
            set { _PaxName = value; }
        }

        public System.String PaxType
        {
            get { return _PaxType; }
            set { _PaxType = value; }
        }

        public System.Int32 Seq
        {
            get { return _Seq; }
            set { _Seq = value; }
        }

        public System.String SelectedSeat
        {
            get { return _SelectedSeat; }
            set { _SelectedSeat = value; }
        }

        public System.String CurrentSeat
        {
            get { return _CurrentSeat; }
            set { _CurrentSeat = value; }
        }

        public string CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }
        public string CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        public string LastAccess
        {
            get { return _LastAccess; }
            set { _LastAccess = value; }
        }
        public string AccessDate
        {
            get { return _AccessDate; }
            set { _AccessDate = value; }
        }
        public System.Byte Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public System.Byte Flag
        {
            get { return _Flag; }
            set { _Flag = value; }
        }

        public System.Int32 PassengerNumber
        {
            get { return _PassengerNumber; }
            set { _PassengerNumber = value; }
        }

        public System.String CompartmentDesignator
        {
            get { return _CompartmentDesignator; }
            set { _CompartmentDesignator = value; }
        }

        public System.String Deck
        {
            get { return _Deck; }
            set { _Deck = value; }
        }

        public System.String Orientation
        {
            get { return _Orientation; }
            set { _Orientation = value; }
        }

        public System.String SeatSet
        {
            get { return _SeatSet; }
            set { _SeatSet = value; }
        }

        public System.Int32 IsHotSeat
        {
            get { return _IsHotSeat; }
            set { _IsHotSeat = value; }
        }

        public System.Decimal SeatAmount
        {
            get { return _SeatAmount; }
            set { _SeatAmount = value; }
        }

        public System.String RecordLocator
        {
            get { return _RecordLocator; }
            set { _RecordLocator = value; }
        }

        public System.String SeatGroup
        {
            get { return _SeatGroup; }
            set { _SeatGroup = value; }
        }
        #endregion
    }

    public class SeatInfos
    {

        ArrayList ArrLst;
        public SeatInfos()
        {
            ArrLst = new ArrayList();
        }

        public void Add(SeatInfo obj)
        {
            ArrLst.Add(obj);
        }

        public int Count()
        {
            return ArrLst.Count;
        }

        public object Item(int index)
        {
            return ArrLst[index];
        }
    }
}
