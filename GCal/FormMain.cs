using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Util;
using GCal.Classes.Algemeen;
using GCal.Classes.Cal.Authentication;
using GCal.Classes.Cal;

namespace GCal
{
    public partial class FormMain : Form
    {
        #region Variables
        /********************************************** Variables ******************************************************************/

        private Kalender m_Kalender;

        private List<Dienst> m_Diensten;

        private List<KalenderItem> m_VerkeerdeItems;

        #endregion Variables

        #region Constructor, Load & Closing
        /********************************************** Constructor, Load & Closing ************************************************/

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            m_Kalender = new Kalender();
            m_VerkeerdeItems = new List<KalenderItem>();

            Init();
        }

        private void Init()
        {
            m_Diensten = new List<Dienst>();

            m_Diensten.Add(new Dienst("I82", 14, 30, 22, 30));
            m_Diensten.Add(new Dienst("I82c", 14, 30, 22, 30));
            m_Diensten.Add(new Dienst("3I82", 15, 15, 23, 15));
            m_Diensten.Add(new Dienst("B82", 7, 0, 15, 0));
            m_Diensten.Add(new Dienst("B82c", 7, 0, 15, 0));
            m_Diensten.Add(new Dienst("C82", 8, 0, 16, 0));
            m_Diensten.Add(new Dienst("C82c", 8, 0, 16, 0));
            m_Diensten.Add(new Dienst("R81", 23, 0, 7, 0));
        }

        #endregion Constructor, Load & Closing

        #region Button Events
        /********************************************** Button Events **************************************************************/

        private void btnTest_Click(object sender, EventArgs e)
        {
            Test();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Update();
        }

        #endregion Button Events

        #region Control Events
        /********************************************** Control Events *************************************************************/

        #endregion Control Events

        #region Functions
        /********************************************** Functions ******************************************************************/

        private void Test()
        {
            try
            {
                if (m_Kalender.GetEvents())
                {
                    foreach (KalenderItem myItem in m_Kalender.Items.OrderBy(i => i.Start))
                    {
                        if (myItem.Omschrijving.IndexOf("82") > -1 || myItem.Omschrijving.IndexOf("81") > -1)
                        {
                            Dienst myDienst = m_Diensten.Where(d => d.Omschrijving == myItem.Omschrijving).Single();

                            bool bTijdVerkeerd = myItem.Start.Hour != myDienst.StartUur || myItem.Start.Minute != myDienst.StartMinuut || myItem.End.Hour != myDienst.EindUur || myItem.End.Minute != myDienst.EindMinuut;

                            string sVerkeerd = "";

                            string myFullStartString = "";

                            if (bTijdVerkeerd)
                            {
                                myFullStartString = myItem.Start.ToString("yyyy-MM-ddTHH:mm:sszzz");
                                myItem.FixDate(myDienst);
                                sVerkeerd = "*";
                            }
                            else
                            {
                                myFullStartString = myItem.Event.Start.DateTime;
                                sVerkeerd = " ";
                            }

                            if (bTijdVerkeerd)
                            {

                                m_VerkeerdeItems.Add(myItem);

                                string myStartString = myItem.Start.ToString("dd-MM-yyyy HH:mm");
                                string myEndString = myItem.End.ToString("dd-MM-yyyy HH:mm");

                                rtbLog.Text += string.Format("{0} - {1}:{2}", myStartString, myEndString, myItem.Omschrijving) + Environment.NewLine;
                                //rtbLog.Text += sVerkeerd + myFullStartString + string.Format(" {0} - {1}:{2}", myStartString, myEndString, myItem.Omschrijving) + Environment.NewLine;

                            }




                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Update()
        {
            m_Kalender.UpdateItems(m_VerkeerdeItems);
        }

        #endregion Functions




        #region Properties
        /********************************************** Properties *****************************************************************/

        #endregion Properties
    }
}
