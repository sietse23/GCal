using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCal.Classes.Cal
{
    public class Dienst
    {
        #region Variables
        /********************************************** Variables ******************************************************************/

        private string m_Omschrijving;

        private int m_StartUur;
        private int m_StartMinuut;
        private int m_EindUur;
        private int m_EindMinuut;

        #endregion Variables

        #region Constructor, Load & Closing
        /********************************************** Constructor, Load & Closing ************************************************/

        public Dienst(string Omschrijving, int StartUur, int StartMinuut, int EindUur, int EindMinuut)
        {
            m_Omschrijving = Omschrijving;
            m_StartUur = StartUur;
            m_StartMinuut = StartMinuut;
            m_EindUur = EindUur;
            m_EindMinuut = EindMinuut;
        }

        #endregion Constructor, Load & Closing

        #region Functions
        /********************************************** Functions ******************************************************************/

        #endregion Functions

        #region Properties
        /********************************************** Properties *****************************************************************/

        public string Omschrijving
        {
            get { return this.m_Omschrijving; }
        }

        public int StartUur
        {
            get { return this.m_StartUur; }
        }

        public int StartMinuut
        {
            get { return this.m_StartMinuut; }
        }

        public int EindUur
        {
            get { return this.m_EindUur; }
        }

        public int EindMinuut
        {
            get { return this.m_EindMinuut; }
        }

        #endregion Properties

    }
}
