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
    public class Kalender
    {
        #region Variables
        /********************************************** Variables ******************************************************************/

        private CalendarService m_Service = null;

        private CalendarListEntry m_Entry = null;

        private Events m_Events = null;

        private List<KalenderItem> m_Items;

        #endregion Variables

        #region Constructor, Load & Closing
        /********************************************** Constructor, Load & Closing ************************************************/

        public Kalender()
        {
            Init();
        }

        private void Init()
        {
            try
            {
                m_Service = new CalendarService(GAuth.CreateAuthenticator());
                m_Items = new List<KalenderItem>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        #endregion Constructor, Load & Closing

        #region Functions
        /********************************************** Functions ******************************************************************/

        public bool GetEvents()
        {
            bool Result = false;

            m_Items.Clear();

            try
            {
                // Fetch all TasksLists of the user asynchronously.
                CalendarList myCalendarList = m_Service.CalendarList.List().Fetch();

                if (myCalendarList != null && myCalendarList.Items != null && myCalendarList.Items.Count > 0)
                {
                    m_Entry = myCalendarList.Items[0];

                    EventsResource.ListRequest myRequest = m_Service.Events.List(m_Entry.Id);

                    myRequest.MaxResults = 10000;

                    m_Events = myRequest.Fetch();

                    foreach (Event myEvent in m_Events.Items)
                    {
                        m_Items.Add(new KalenderItem(myEvent));
                    }

                    Result = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return Result;
        }

        public void UpdateItems(List<KalenderItem> myItems)
        {
            foreach (KalenderItem myItem in myItems)
            {
                try
                {
                    EventsResource.UpdateRequest myUpdateRequest = new EventsResource.UpdateRequest(m_Service, myItem.Event, m_Entry.Id, myItem.Event.Id);
                    myUpdateRequest.Fetch();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }

        #endregion Functions

        #region Properties
        /********************************************** Properties *****************************************************************/

        public Events Events 
        {
            get { return this.m_Events; }
        }

        public List<KalenderItem> Items
        {
            get { return this.m_Items; }
        }
            
        #endregion Properties

    }
}
