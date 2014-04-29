using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Util;
using GCal.Classes.Algemeen;
using GCal.Classes.Cal.Authentication;
using System.Windows.Forms;

namespace GCal.Classes.Cal
{
    public class KalenderItem
    {
        #region Variables
        /********************************************** Variables ******************************************************************/

        private Event m_Event;

        private DateTime m_Start;
        private DateTime m_End;

        private string m_Omschrijving;

        #endregion Variables

        #region Constructor, Load & Closing
        /********************************************** Constructor, Load & Closing ************************************************/

        public KalenderItem(Event Event)
        {
            m_Event = Event;

            Init();
        }

        #endregion Constructor, Load & Closing

        #region Functions
        /********************************************** Functions ******************************************************************/

        private void Init()
        {
            m_Omschrijving = "";

            if (!string.IsNullOrEmpty(m_Event.Description))
            {
                m_Omschrijving = m_Event.Description;
            }
            else if (!string.IsNullOrEmpty(m_Event.Summary))
            {
                m_Omschrijving = m_Event.Summary;
            }

            m_Start = DateTime.Now;
            m_End = DateTime.Now;

            if (!string.IsNullOrEmpty(m_Event.Start.DateTime))
            {
                m_Start = Convert.ToDateTime(m_Event.Start.DateTime);
            }
            else if (!string.IsNullOrEmpty(m_Event.Start.Date))
            {
                m_Start = Convert.ToDateTime(m_Event.Start.Date);
            }

            if (!string.IsNullOrEmpty(m_Event.End.DateTime))
            {
                m_End = Convert.ToDateTime(m_Event.End.DateTime);
            }
            else if (!string.IsNullOrEmpty(m_Event.End.Date))
            {
                m_End = Convert.ToDateTime(m_Event.End.Date);
            }
        }

        public void FixDate(Dienst myDienst)
        {            
            if (myDienst.Omschrijving != "R81")
            {
                if (m_End.Day > m_Start.Day || m_End.Month > m_Start.Month)
                {
                    m_End = m_End.AddDays(-1);
                }
            }

            m_Start = new DateTime(m_Start.Year, m_Start.Month, m_Start.Day, myDienst.StartUur, myDienst.StartMinuut, 0);
            m_End = new DateTime(m_End.Year, m_End.Month, m_End.Day, myDienst.EindUur, myDienst.EindMinuut, 0);

            m_Event.Start.Date = null;
            //m_Event.Start.DateTime = m_Start.ToString("yyyy-MM-ddTHH:mm:ss+02:00");
            m_Event.Start.DateTime = m_Start.ToString("yyyy-MM-ddTHH:mm:sszzz");

            m_Event.End.Date = null;
            //m_Event.End.DateTime = m_End.ToString("yyyy-MM-ddTHH:mm:ss+02:00");
            m_Event.End.DateTime = m_End.ToString("yyyy-MM-ddTHH:mm:sszzz");

            Init();
        }

        #endregion Functions

        #region Properties
        /********************************************** Properties *****************************************************************/

        public string Omschrijving
        {
            get { return this.m_Omschrijving; }
        }

        public DateTime Start
        {
            get { return this.m_Start; }
            set { this.m_Start = value; }
        }

        public DateTime End
        {
            get { return this.m_End; }
            set { this.m_End = value; }
        }

        public Event Event
        {
            get { return this.m_Event; }
            set { this.m_Event = value; }
        }
        
        #endregion Properties

    }
}
