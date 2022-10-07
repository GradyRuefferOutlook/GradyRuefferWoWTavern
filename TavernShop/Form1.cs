using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.Runtime.ConstrainedExecution;
using System.IO;

namespace TavernShop
    //ENSURE THE FONTS ARE INSTALLED
{   //Items can be bought by selecting them in the shop menu, they can also be old by opening the inventory and clicking on an item
    //It is therefore possible for the innkeeper to owe you money

    //Concurrent Audio Not Yet Working

    //Easter Egg Solution Hidden at Bottom
    //Hint: Leeeeeroy Jennnkins! A well-known line by many from an old, sadly staged, but hilarious Youtube Video.
    //Truly, it has stood the test of time as it is even older than me, yet still well known. When did it come out again?
    public partial class DwarvenTavern : Form
    {
        //Form1 sizes to show or hide the shop by expanding or shrinking
        Size noShop = new Size(600, 660);
        Size shop = new Size(990, 660);

        //To store the current purchases, sales, and the previous purchased items
        int footeTripelCounter, dwarvenMeadCounter, lagraveStoutCounter, springWaterCounter, beerBastedBoarRibsCounter, rhapsodyMaltCounter, hearthglenAmbrosiaCounter, melonJuiceCounter;
        int footeTripelSellCounter, dwarvenMeadSellCounter, lagraveStoutSellCounter, springWaterSellCounter, beerBastedBoarRibsSellCounter, rhapsodyMaltSellCounter, hearthglenAmbrosiaSellCounter, melonJuiceSellCounter;
        int footeTripelPrevCounter, dwarvenMeadPrevCounter, lagraveStoutPrevCounter, springWaterPrevCounter, beerBastedBoarRibsPrevCounter, rhapsodyMaltPrevCounter, hearthglenAmbrosiaPrevCounter, melonJuicePrevCounter;

        //Random generator for the background and innkeeper name
        Random rnd = new Random();

        //Create Backgrounds to pick from
        Image[] backgrounds = new Image[9];

        //Random Dwarf names to pick from
        string[] dwarfNames = {
        "Throssarlig Steelshoulder",
        "Omour Kegbreaker",
        "Fonmean Kegmace",
        "Umidrear Iceborn",
        "Buthat Duskfoot",
        "Ruddath Bloodcloak",
        "Alfogherlum Oakbelt",
        "Snamdouc Leadflayer",
        "Dammear Snowbranch",
        "Thakdrum Kegbow",
        "Jargamli Duskbrow",
        "Datromli Embergranite",
        "Werreal Grayhead",
        "Throstroth Ambergut",
        "Snazumir Metalbrewer",
        "Strorfuc Gravelfury",
        "Dangrul Greatbane",
        "Groostead Hammerborn",
        "Whutdrorli Hammerbrew",
        "Derbag Axeriver",
        "Grarnor Pebblegranite",
        "Darerberlig Brickgrip",
        "Durfik Runetoe",
        "Thatmaeck Goldforge",
        "Douznotir Windmail",
        "Khuvonlim Barrelbeard",
        "Malgem Greycoat",
        "Brounmarlun Caskhead",
        "Kirheath Largesunder",
        "Jostroth Blessedborn"};

        //Declare global soundplayers
        SoundPlayer tavernMusic = new SoundPlayer(Properties.Resources.BackgroundMusic);
        public DwarvenTavern()
        {
            InitializeComponent();
            sizeInit.Start();
            
            //Corrects any user inputs, see method for details
            inputCorrector.Start();

            //Start Playing music
            TavernLoop.Start();
            tavernMusic.Play();

            //Generate a random dwarf name to display
            dwarfName();

            //Set the possible backgrounds
            backgrounds[0] = Properties.Resources.TavernBackground;
            backgrounds[1] = Properties.Resources.TavernBackground2;
            backgrounds[2] = Properties.Resources.TavernBackground3;
            backgrounds[3] = Properties.Resources.TavernBackground4;
            backgrounds[4] = Properties.Resources.TavernBackground5;
            backgrounds[5] = Properties.Resources.TavernBackground6;
            backgrounds[6] = Properties.Resources.TavernBackground7;
            backgrounds[7] = Properties.Resources.TavernBackground8;
            backgrounds[8] = Properties.Resources.TavernBackground9;

            //Set the chosen background
            shopWindow.Parent = backgroundTavern;
            speechWindowNotInteractive.Parent = backgroundTavern;
            speechWindowNotInteractive.Location = new Point(-4, -4);

            //Below is the parenting and locating of all the components
            //Trying to read everything is ill-advised, they are organized but are far too numberous
            goodbyePress.Parent = speechWindowNotInteractive;
            goodbyePress.Location = new Point(305, 439);

            xButtonLeft.Parent = speechWindowNotInteractive;
            dwarfTalkLabel.Parent = speechWindowNotInteractive;
            speechBubbleButton.Parent = speechWindowNotInteractive;
            leeroySpeechBubbleButton.Parent = speechWindowNotInteractive;
            leeroySpeechLabel.Parent = speechWindowNotInteractive;
            shopBagButton.Parent = speechWindowNotInteractive;
            xButtonRight.Parent = shopWindow;
            xButtonRight.Location = new Point(88, 20);

            dwarfDisplayLeft.Parent = speechWindowNotInteractive;
            dwarfDisplayRight.Parent = shopWindow;
            dwarfDisplayRight.Location = new Point(352, 0);

            dwarfNameLeft.Parent = speechWindowNotInteractive;
            dwarfNameRight.Parent = shopWindow;
            dwarfNameRight.Location = new Point(125, 20);

            speechLabel.Parent = speechWindowNotInteractive;
            waresLabel.Parent = speechWindowNotInteractive;

            callInkeeperButton.Parent = backgroundTavern;

            xButtonBag.Parent = inventoryWindow;
            xButtonBag.Location = new Point(340, 5);

            xButtonBag.Visible = false;
            inventoryWindow.Visible = false;

            beerBastedBoarRibsDisplay.Parent = shopWindow;
            legraveStoutDisplay.Parent = shopWindow;
            melonJuiceDisplay.Parent = shopWindow;
            dwarvenMeadDisplay.Parent = shopWindow;
            rhapsodyMaltDisplay.Parent = shopWindow;
            //dwarfDisplayRight would normally cover this display
            footeTripelDisplay.Parent = dwarfDisplayRight;
            springWaterDisplay.Parent = shopWindow;
            hearthglenAmbrosiaDisplay.Parent = shopWindow;

            beerBastedBoarRibsDisplay.Location = new Point(208, 85);
            springWaterDisplay.Location = new Point(208, 134);
            hearthglenAmbrosiaDisplay.Location = new Point(208, 184);
            melonJuiceDisplay.Location = new Point(208, 235);

            footeTripelDisplay.Location = new Point(16, 85);
            rhapsodyMaltDisplay.Location = new Point(368, 134);
            dwarvenMeadDisplay.Location = new Point(368, 184);
            legraveStoutDisplay.Location = new Point(368, 234);

            shopLabel1.Parent = shopWindow;
            shopLabel2.Parent = shopWindow;
            shopLabel3.Parent = shopWindow;
            shopLabel4.Parent = shopWindow;
            shopLabel5.Parent = shopWindow;
            shopLabel6.Parent = shopWindow;
            shopLabel7.Parent = shopWindow;
            shopLabel8.Parent = shopWindow;

            shopLabel1.Location = new Point(100, 82);
            shopLabel3.Location = new Point(100, 131);
            shopLabel5.Location = new Point(100, 180);
            shopLabel7.Location = new Point(100, 229);
            shopLabel2.Location = new Point(251, 82);
            shopLabel4.Location = new Point(251, 131);
            shopLabel6.Location = new Point(251, 180);
            shopLabel8.Location = new Point(251, 229);

            gold1.Parent = shopWindow;
            gold2.Parent = shopWindow;
            gold3.Parent = shopWindow;
            gold4.Parent = shopWindow;
            gold5.Parent = shopWindow;
            gold6.Parent = shopWindow;
            gold7.Parent = shopWindow;
            gold8.Parent = shopWindow;

            goldCost1.Parent = shopWindow;
            goldCost2.Parent = shopWindow;
            goldCost3.Parent = shopWindow;
            goldCost4.Parent = shopWindow;
            goldCost5.Parent = shopWindow;
            goldCost6.Parent = shopWindow;
            goldCost7.Parent = shopWindow;
            goldCost8.Parent = shopWindow;

            silver1.Parent = shopWindow;
            silver2.Parent = shopWindow;
            silver3.Parent = shopWindow;
            silver4.Parent = shopWindow;
            silver5.Parent = shopWindow;
            silver6.Parent = shopWindow;
            silver7.Parent = shopWindow;
            silver8.Parent = shopWindow;

            silverAmount1.Parent = shopWindow;
            silverAmount2.Parent = shopWindow;
            silverAmount3.Parent = shopWindow;
            silverAmount4.Parent = shopWindow;
            silverAmount5.Parent = shopWindow;
            silverAmount6.Parent = shopWindow;
            silverAmount7.Parent = shopWindow;
            silverAmount8.Parent = shopWindow;

            copper1.Parent = shopWindow;
            copper2.Parent = shopWindow;
            copper3.Parent = shopWindow;
            copper4.Parent = shopWindow;
            copper5.Parent = shopWindow;
            copper6.Parent = shopWindow;
            copper7.Parent = shopWindow;
            copper8.Parent = shopWindow;

            copperCost1.Parent = shopWindow;
            copperCost2.Parent = shopWindow;
            copperCost3.Parent = shopWindow;
            copperCost4.Parent = shopWindow;
            copperCost5.Parent = shopWindow;
            copperCost6.Parent = shopWindow;
            copperCost7.Parent = shopWindow;
            copperCost8.Parent = shopWindow;

            gold1.Location = new Point(95, 115);
            gold2.Location = new Point(249, 115);
            gold3.Location = new Point(95, 164);
            gold4.Location = new Point(249, 164);
            gold5.Location = new Point(95, 213);
            gold6.Location = new Point(249, 213);
            gold7.Location = new Point(95, 252);
            gold8.Location = new Point(249, 252);

            goldCost1.Location = new Point(105, 115);
            goldCost2.Location = new Point(259, 115);
            goldCost3.Location = new Point(105, 164);
            goldCost4.Location = new Point(259, 164);
            goldCost5.Location = new Point(105, 213);
            goldCost6.Location = new Point(259, 213);
            goldCost7.Location = new Point(105, 252);
            goldCost8.Location = new Point(259, 252);

            silver1.Location = new Point(130, 115);
            silver2.Location = new Point(284, 115);
            silver3.Location = new Point(130, 164);
            silver4.Location = new Point(284, 164);
            silver5.Location = new Point(130, 213);
            silver6.Location = new Point(284, 213);
            silver7.Location = new Point(130, 252);
            silver8.Location = new Point(284, 252);

            silverAmount1.Location = new Point(140, 115);
            silverAmount2.Location = new Point(294, 115);
            silverAmount3.Location = new Point(140, 164);
            silverAmount4.Location = new Point(294, 164);
            silverAmount5.Location = new Point(140, 213);
            silverAmount6.Location = new Point(294, 213);
            silverAmount7.Location = new Point(140, 252);
            silverAmount8.Location = new Point(294, 252);

            copper1.Location = new Point(175, 115);
            copper2.Location = new Point(329, 115);
            copper3.Location = new Point(175, 164);
            copper4.Location = new Point(329, 164);
            copper5.Location = new Point(175, 213);
            copper6.Location = new Point(329, 213);
            copper7.Location = new Point(175, 252);
            copper8.Location = new Point(329, 252);

            copperCost1.Location = new Point(185, 115);
            copperCost2.Location = new Point(339, 115);
            copperCost3.Location = new Point(185, 164);
            copperCost4.Location = new Point(339, 164);
            copperCost5.Location = new Point(185, 213);
            copperCost6.Location = new Point(339, 213);
            copperCost7.Location = new Point(185, 252);
            copperCost8.Location = new Point(339, 252);

            tenderedLabel.Parent = shopWindow;
            goldTenderedDisplay.Parent = shopWindow;
            goldTenderedInput.Parent = shopWindow;
            silverTenderedDisplay.Parent = shopWindow;
            silverTenderedInput.Parent = shopWindow;
            copperTenderedDisplay.Parent = shopWindow;
            copperTenderedInput.Parent = shopWindow;

            tenderedLabel.Location = new Point(135, 432);
            goldTenderedDisplay.Location = new Point(257, 432);
            goldTenderedInput.Location = new Point(270, 432);
            silverTenderedDisplay.Location = new Point(302, 432);
            silverTenderedInput.Location = new Point(315, 432);
            copperTenderedDisplay.Location = new Point(357, 432);
            copperTenderedInput.Location = new Point(370, 432);

            receiptButton.Parent = shopWindow;
            receiptButton.Location = new Point(341, 455);

            inventoryLabel.Parent = inventoryWindow;
            inventoryLabel.Location = new Point(25, 0);
            inventoryPageLabel.Parent = inventoryWindow;
            inventoryPageLabel.Location = new Point(15, 360);

            inventoryItem1.Parent = inventoryWindow;
            inventoryItemLabel1.Parent = inventoryItem1;
            inventoryItem2.Parent = inventoryWindow;
            inventoryItemLabel2.Parent = inventoryItem2;
            inventoryItem3.Parent = inventoryWindow;
            inventoryItemLabel3.Parent = inventoryItem3;
            inventoryItem4.Parent = inventoryWindow;
            inventoryItemLabel4.Parent = inventoryItem4;
            inventoryItem5.Parent = inventoryWindow;
            inventoryItemLabel5.Parent = inventoryItem5;
            inventoryItem6.Parent = inventoryWindow;
            inventoryItemLabel6.Parent = inventoryItem6;
            inventoryItem7.Parent = inventoryWindow;
            inventoryItemLabel7.Parent = inventoryItem7;
            inventoryItem8.Parent = inventoryWindow;
            inventoryItemLabel8.Parent = inventoryItem8;

            inventoryItem1.Location = new Point(8, 35);
            inventoryItem2.Location = new Point(53, 35);
            inventoryItem3.Location = new Point(98, 35);
            inventoryItem4.Location = new Point(143, 35);
            inventoryItem5.Location = new Point(187, 35);
            inventoryItem6.Location = new Point(232, 35);
            inventoryItem7.Location = new Point(276, 35);
            inventoryItem8.Location = new Point(321, 35);

            inventoryItemLabel1.Location = new Point(0, 25);
            inventoryItemLabel2.Location = new Point(0, 25);
            inventoryItemLabel3.Location = new Point(0, 25);
            inventoryItemLabel4.Location = new Point(0, 25);
            inventoryItemLabel5.Location = new Point(0, 25);
            inventoryItemLabel6.Location = new Point(0, 25);
            inventoryItemLabel7.Location = new Point(0, 25);
            inventoryItemLabel8.Location = new Point(0, 25);

            inventoryItem1.Visible = false;
            inventoryItem2.Visible = false;
            inventoryItem3.Visible = false;
            inventoryItem4.Visible = false;
            inventoryItem5.Visible = false;
            inventoryItem6.Visible = false;
            inventoryItem7.Visible = false;
            inventoryItem8.Visible = false;

            inventoryItemLabel1.Text = "";
            inventoryItemLabel2.Text = "";
            inventoryItemLabel3.Text = "";
            inventoryItemLabel4.Text = "";
            inventoryItemLabel5.Text = "";
            inventoryItemLabel6.Text = "";
            inventoryItemLabel7.Text = "";
            inventoryItemLabel8.Text = "";

            inventoryItemLabel1.Visible = false;
            inventoryItemLabel2.Visible = false;
            inventoryItemLabel3.Visible = false;
            inventoryItemLabel4.Visible = false;
            inventoryItemLabel5.Visible = false;
            inventoryItemLabel6.Visible = false;
            inventoryItemLabel7.Visible = false;
            inventoryItemLabel8.Visible = false;

            costCalculateButton.Parent = shopWindow;
            goldOrderCost.Parent = shopWindow;
            goldOrderCostDisplay.Parent = shopWindow;
            silverOrderCost.Parent = shopWindow;
            silverOrderCostDisplay.Parent = shopWindow;
            copperOrderCost.Parent = shopWindow;
            copperOrderCostDisplay.Parent = shopWindow;

            costCalculateButton.Location = new Point(100, 290);
            goldOrderCost.Location = new Point(117, 345);
            goldOrderCostDisplay.Location = new Point(100, 345);
            silverOrderCost.Location = new Point(169, 345);
            silverOrderCostDisplay.Location = new Point(152, 345);
            copperOrderCost.Location = new Point(226, 345);
            copperOrderCostDisplay.Location = new Point(211, 345);

            receiptDisplay.Location = new Point(414, 660);
            rightSideReceiptLabel.Parent = receiptDisplay;
            leftSideReceiptLabel.Parent = receiptDisplay;
            rightSideReceiptLabel.Location = new Point(110, 66);
            leftSideReceiptLabel.Location = new Point(0, 66);
        }

        private void correctInput(object sender, EventArgs e)
        {
            //Runs the tendered corrections function to fix the input before it returns an incorrect input
            tenderedCorrections();
        }

        private void leeroySpeechBubbleButton_Click(object sender, EventArgs e)
        {
            if (leeroySpeechLabel.Text == "Leeeeroy Jennnnkins!")
            {
                dwarfTalkLabel.Text = "Oh my god he just ran in!\nStick to the plan!\nGod damn it Leeroy!";
                leeroySpeechLabel.Text = "<It's Not My Fault!>";
            }
            else if (leeroySpeechLabel.Text == "<It's Not My Fault!>")
            {
                dwarfTalkLabel.Text = "Welcome Traveller!\n     Might I interest ye in a pint...\nor perhaps an imported delight!\n     Something to clear ye head\nor warm ye belly!";
                leeroySpeechLabel.Text = "Leeeeroy Jennnnkins!";
                speechLabel.Text = "Where are we?";
            }
        }

        void tenderedCorrections()
        {
            try
            {
                if ((goldTenderedInput.Text == "") == false)
                {
                    Convert.ToInt32(goldTenderedInput.Text);
                }
                if (Convert.ToInt32(goldTenderedInput.Text) < 0)
                {
                    goldTenderedInput.Text = "0";
                }
            }
            catch
            {
                goldTenderedInput.Text = "0";
            }

            try
            {
                if ((silverTenderedInput.Text == "") == false)
                {
                    Convert.ToInt32(silverTenderedInput.Text);
                }
                if (Convert.ToInt32(silverTenderedInput.Text) < 0)
                {
                    silverTenderedInput.Text = "0";
                }
            }
            catch
            {
                silverTenderedInput.Text = "0";
            }

            try
            {
                if ((copperTenderedInput.Text == "") == false)
                {
                    Convert.ToInt32(copperTenderedInput.Text);
                }
                if (Convert.ToInt32(copperTenderedInput.Text) < 0)
                {
                    copperTenderedInput.Text = "0";
                }
            }
            catch
            {
                copperTenderedInput.Text = "0";
            }
        }

        private void generateReceipt(object sender, EventArgs e)
        {
            //Ensure the label is correct if input was previously incorrect
            tenderedLabel.Text = ":Tendered:";

            tenderedCorrections();
            Int64[] footeTripelPrice = { 2, 56, 0 }, dwarvenMeadPrice = { 0, 15, 0 }, lagraveStoutPrice = { 2, 24, 0 }, springWaterPrice = { 0, 0, 25 }, beerBastedBoarRibsPrice = { 0, 10, 55 }, rhapsodyMaltPrice = { 0, 0, 50 }, hearthglenAmbrosiaPrice = { 2, 56, 0 }, melonJuicePrice = { 0, 5, 0 };

            Int64 totalTendered = (((Convert.ToInt64(goldTenderedInput.Text)) * 10000) + ((Convert.ToInt64(silverTenderedInput.Text)) * 100) + (Convert.ToInt64(copperTenderedInput.Text)));

            Int64 ribsPrice = beerBastedBoarRibsCounter * ((beerBastedBoarRibsPrice[0] * 10000) + (beerBastedBoarRibsPrice[1] * 100) + (beerBastedBoarRibsPrice[2]));
            Int64 ambrosiaPrice = hearthglenAmbrosiaCounter * ((hearthglenAmbrosiaPrice[0] * 10000) + (hearthglenAmbrosiaPrice[1] * 100) + (hearthglenAmbrosiaPrice[2]));
            Int64 lagravePrice = lagraveStoutCounter * ((lagraveStoutPrice[0] * 10000) + (lagraveStoutPrice[1] * 100) + (lagraveStoutPrice[2]));
            Int64 meadPrice = dwarvenMeadCounter * ((dwarvenMeadPrice[0] * 10000) + (dwarvenMeadPrice[1] * 100) + (dwarvenMeadPrice[2]));
            Int64 waterPrice = springWaterCounter * ((springWaterPrice[0] * 10000) + (springWaterPrice[1] * 100) + (springWaterPrice[2]));
            Int64 maltPrice = rhapsodyMaltCounter * ((rhapsodyMaltPrice[0] * 10000) + (rhapsodyMaltPrice[1] * 100) + (rhapsodyMaltPrice[2]));
            Int64 footePrice = footeTripelCounter * ((footeTripelPrice[0] * 10000) + (footeTripelPrice[1] * 100) + (footeTripelPrice[2]));
            Int64 melonPrice = melonJuiceCounter * ((melonJuicePrice[0] * 10000) + (melonJuicePrice[1] * 100) + (melonJuicePrice[2]));

            Int64 totalCost = ribsPrice + ambrosiaPrice + lagravePrice + meadPrice + waterPrice + maltPrice + footePrice + melonPrice;

            Int64 ribsSoldPrice = beerBastedBoarRibsSellCounter * ((beerBastedBoarRibsPrice[0] * 10000) + (beerBastedBoarRibsPrice[1] * 100) + (beerBastedBoarRibsPrice[2]));
            Int64 ambrosiaSoldPrice = hearthglenAmbrosiaSellCounter * ((hearthglenAmbrosiaPrice[0] * 10000) + (hearthglenAmbrosiaPrice[1] * 100) + (hearthglenAmbrosiaPrice[2]));
            Int64 lagraveSoldPrice = lagraveStoutSellCounter * ((lagraveStoutPrice[0] * 10000) + (lagraveStoutPrice[1] * 100) + (lagraveStoutPrice[2]));
            Int64 meadSoldPrice = dwarvenMeadSellCounter * ((dwarvenMeadPrice[0] * 10000) + (dwarvenMeadPrice[1] * 100) + (dwarvenMeadPrice[2]));
            Int64 waterSoldPrice = springWaterSellCounter * ((springWaterPrice[0] * 10000) + (springWaterPrice[1] * 100) + (springWaterPrice[2]));
            Int64 maltSoldPrice = rhapsodyMaltSellCounter * ((rhapsodyMaltPrice[0] * 10000) + (rhapsodyMaltPrice[1] * 100) + (rhapsodyMaltPrice[2]));
            Int64 footeSoldPrice = footeTripelSellCounter * ((footeTripelPrice[0] * 10000) + (footeTripelPrice[1] * 100) + (footeTripelPrice[2]));
            Int64 melonSoldPrice = melonJuiceSellCounter * ((melonJuicePrice[0] * 10000) + (melonJuicePrice[1] * 100) + (melonJuicePrice[2]));

            Int64 totalSold = ribsSoldPrice + ambrosiaSoldPrice + lagraveSoldPrice + meadSoldPrice + waterSoldPrice + maltSoldPrice + footeSoldPrice + melonSoldPrice;

            Int64 fullCost = totalCost - totalSold;

            string receiptItem1 = "0";
            string receiptItem2 = "0";
            string receiptItem3 = "0";
            string receiptItem4 = "0";
            string receiptItem5 = "0";
            string receiptItem6 = "0";
            string receiptItem7 = "0";
            string receiptItem8 = "0";

            Int64 taxAmount = Convert.ToInt32(fullCost * 0.13);
            Int64 totalCostWTax = fullCost + taxAmount;

            Int64 change = totalTendered - totalCostWTax;

            bool costCompare = (totalCostWTax) <= (totalTendered);

            if (costCompare == true)
            {
                tenderedCorrections();

                if (goldTenderedInput.Text == "")
                {
                    goldTenderedInput.Text = "0";
                }

                if (silverTenderedInput.Text == "")
                {
                    silverTenderedInput.Text = "0";
                }

                if (copperTenderedInput.Text == "")
                {
                    copperTenderedInput.Text = "0";
                }

                if (inventoryItem1.Tag == "1")
                {
                    receiptItem1 = inventoryItemReader(inventoryItem1);
                }
                if (inventoryItem2.Tag == "1")
                {
                    receiptItem2 = inventoryItemReader(inventoryItem2);
                }
                if (inventoryItem3.Tag == "1")
                {
                    receiptItem3 = inventoryItemReader(inventoryItem3);
                }
                if (inventoryItem4.Tag == "1")
                {
                    receiptItem4 = inventoryItemReader(inventoryItem4);
                }
                if (inventoryItem5.Tag == "1")
                {
                    receiptItem5 = inventoryItemReader(inventoryItem5);
                }
                if (inventoryItem6.Tag == "1")
                {
                    receiptItem6 = inventoryItemReader(inventoryItem6);
                }
                if (inventoryItem7.Tag == "1")
                {
                    receiptItem7 = inventoryItemReader(inventoryItem7);
                }
                if (inventoryItem8.Tag == "1")
                {
                    receiptItem8 = inventoryItemReader(inventoryItem8);
                }

                int[] counterValues = { inventoryItemLabelReader(inventoryItem1), inventoryItemLabelReader(inventoryItem2), inventoryItemLabelReader(inventoryItem3), inventoryItemLabelReader(inventoryItem4), inventoryItemLabelReader(inventoryItem5), inventoryItemLabelReader(inventoryItem6), inventoryItemLabelReader(inventoryItem7), inventoryItemLabelReader(inventoryItem8) };
                int[] leeroyJenkins = { 1, 1, 0, 5, 2, 0, 0, 5 }; //May 11th, 2005

                int leeroyCheck = 0;
                for (int o = 0; o <= 7; o++)
                {
                    if (counterValues[o] == leeroyJenkins[o])
                    {
                        leeroyCheck = leeroyCheck + 1;
                    }
                    else
                    {
                        o = 8;
                    }

                    if (leeroyCheck == 8)
                    {
                        leeroySpeechLabel.Visible = true;
                        leeroySpeechBubbleButton.Visible = true;
                    }
                }

                leftSideReceiptLabel.Text = "Purchased:\n";
                rightSideReceiptLabel.Text = "\n";

                if (inventoryItem1.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem1}";
                    receiptRightSidePrinter(inventoryItem1, false);
                }
                if (inventoryItem2.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem2}";
                    receiptRightSidePrinter(inventoryItem2, false);
                }
                if (inventoryItem3.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem3}";
                    receiptRightSidePrinter(inventoryItem3, false);
                }

                if (inventoryItem4.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem4}";
                    receiptRightSidePrinter(inventoryItem4, false);
                }

                if (inventoryItem5.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem5}";
                    receiptRightSidePrinter(inventoryItem5, false);
                }

                if (inventoryItem6.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem6}";
                    receiptRightSidePrinter(inventoryItem6, false);
                }

                if (inventoryItem7.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem7}";
                    receiptRightSidePrinter(inventoryItem7, false);
                }

                if (inventoryItem8.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem8}";
                    receiptRightSidePrinter(inventoryItem8, false);
                }

                leftSideReceiptLabel.Text += "\n\nPurchased Total:";
                rightSideReceiptLabel.Text += $"\n\n${convertPrice(totalCost)}";

                leftSideReceiptLabel.Text += "\n\n\nSold: ";
                rightSideReceiptLabel.Text += "\n\n\n ";

                if (inventoryItem1.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem1}";
                    receiptRightSidePrinter(inventoryItem1, true);
                }

                if (inventoryItem2.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem2}";
                    receiptRightSidePrinter(inventoryItem2, true);
                }

                if (inventoryItem3.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem3}";
                    receiptRightSidePrinter(inventoryItem3, true);
                }

                if (inventoryItem4.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem4}";
                    receiptRightSidePrinter(inventoryItem4, true);
                }

                if (inventoryItem5.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem5}";
                    receiptRightSidePrinter(inventoryItem5, true);
                }

                if (inventoryItem6.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem6}";
                    receiptRightSidePrinter(inventoryItem6, true);
                }

                if (inventoryItem7.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem7}";
                    receiptRightSidePrinter(inventoryItem7, true);
                }

                if (inventoryItem8.Tag == "1")
                {
                    leftSideReceiptLabel.Text += $"\n{receiptItem8}";
                    receiptRightSidePrinter(inventoryItem8, true);
                }

                leftSideReceiptLabel.Text += "\n\nSell Total:";
                rightSideReceiptLabel.Text += $"\n\n{convertPrice(totalSold)}";

                leftSideReceiptLabel.Text += "\n\n\nTendered:";
                rightSideReceiptLabel.Text += "\n\n\n";

                leftSideReceiptLabel.Text += "\n\nAmmount:";
                rightSideReceiptLabel.Text += $"\n\n${convertPrice(totalTendered)}";

                leftSideReceiptLabel.Text += "\n\n\nTotals:";
                rightSideReceiptLabel.Text += "\n\n\n ";

                leftSideReceiptLabel.Text += "\n\nSubTotal:";
                rightSideReceiptLabel.Text += $"\n\n${convertPrice(fullCost)}";

                leftSideReceiptLabel.Text += "\nTax:";
                rightSideReceiptLabel.Text += $"\n${convertPrice(taxAmount)}";

                leftSideReceiptLabel.Text += "\nTotal:";
                rightSideReceiptLabel.Text += $"\n${convertPrice(totalCostWTax)}";

                leftSideReceiptLabel.Text += "\nChange:";
                rightSideReceiptLabel.Text += $"\n${convertPrice(change)}";

                footeTripelPrevCounter = footeTripelCounter;
                dwarvenMeadPrevCounter = dwarvenMeadCounter;
                lagraveStoutPrevCounter = lagraveStoutCounter;
                springWaterPrevCounter = springWaterCounter;
                beerBastedBoarRibsPrevCounter = beerBastedBoarRibsCounter;
                rhapsodyMaltPrevCounter = rhapsodyMaltCounter;
                hearthglenAmbrosiaPrevCounter = hearthglenAmbrosiaCounter;
                melonJuicePrevCounter = melonJuiceCounter;

                footeTripelPrevCounter = 0;
                dwarvenMeadPrevCounter = 0;
                lagraveStoutPrevCounter = 0;
                springWaterPrevCounter = 0;
                beerBastedBoarRibsPrevCounter = 0;
                rhapsodyMaltPrevCounter = 0;
                hearthglenAmbrosiaPrevCounter = 0;
                melonJuicePrevCounter = 0;

                footeTripelCounter = 0;
                dwarvenMeadCounter = 0;
                lagraveStoutCounter = 0;
                springWaterCounter = 0;
                beerBastedBoarRibsCounter = 0;
                rhapsodyMaltCounter = 0;
                hearthglenAmbrosiaCounter = 0;
                melonJuiceCounter = 0;

                receiptDisplay.Tag = 0;
                receiptDisplay.Location = new Point(414, 660);
                receiptPrinter.Start();
                TavernLoop.Stop();
                tavernMusic.Stop();
            }
            else
            {
                tenderedLabel.Text = "Insufficient";
            }
        }

        private void receiptPrinter_Tick(object sender, EventArgs e)
        {
            //Used to print the receipt
            //Using Height of form, move upwards in increments

            SoundPlayer scribble = new SoundPlayer(Properties.Resources.ReceiptScribble);
            scribble.Stop();
            scribble.Play();

            Point receiptLocation = new Point(414, (660 - (Convert.ToInt16(receiptDisplay.Tag) * 20)));
            receiptDisplay.Location = receiptLocation;

            //Moving Labels with receipt
            rightSideReceiptLabel.Location = new Point(110, 66);
            leftSideReceiptLabel.Location = new Point(0, 66);
            receiptDisplay.Tag = (Convert.ToInt64(receiptDisplay.Tag)) + 1;

            //Once near top, Stop
            receiptPrinter.Tag = 34;
            if (Convert.ToInt16(receiptDisplay.Tag) >= Convert.ToInt16(receiptPrinter.Tag)) 
            {
                receiptPrinter.Stop();
                TavernLoop.Start();
                tavernMusic.Play();
            }
        } 

        private void ripReceipt(object sender, EventArgs e)
        {
            //Click Receipt to make it disappear (Hide below screen)
            Point receiptLocation = new Point(414, 660);
            receiptPrinter.Stop();
            receiptDisplay.Location = receiptLocation;
            receiptDisplay.Tag = "0";
            rightSideReceiptLabel.Location = new Point(110, 66);
            leftSideReceiptLabel.Location = new Point(0, 66);
        }

        void receiptRightSidePrinter(Control selection, bool sell)
        {
            //Declare item prices and item sold prices
            int[] footeTripelPrice = { 2, 56, 0 }, dwarvenMeadPrice = { 0, 15, 0 }, lagraveStoutPrice = { 2, 24, 0 }, springWaterPrice = { 0, 0, 25 }, beerBastedBoarRibsPrice = { 0, 10, 55 }, rhapsodyMaltPrice = { 0, 0, 50 }, hearthglenAmbrosiaPrice = { 2, 56, 0 }, melonJuicePrice = { 0, 5, 0 };

            //Calculate item prices
            int ribsPrice = beerBastedBoarRibsCounter * ((beerBastedBoarRibsPrice[0] * 10000) + (beerBastedBoarRibsPrice[1] * 100) + (beerBastedBoarRibsPrice[2]));
            int ambrosiaPrice = hearthglenAmbrosiaCounter * ((hearthglenAmbrosiaPrice[0] * 10000) + (hearthglenAmbrosiaPrice[1] * 100) + (hearthglenAmbrosiaPrice[2]));
            int lagravePrice = lagraveStoutCounter * ((lagraveStoutPrice[0] * 10000) + (lagraveStoutPrice[1] * 100) + (lagraveStoutPrice[2]));
            int meadPrice = dwarvenMeadCounter * ((dwarvenMeadPrice[0] * 10000) + (dwarvenMeadPrice[1] * 100) + (dwarvenMeadPrice[2]));
            int waterPrice = springWaterCounter * ((springWaterPrice[0] * 10000) + (springWaterPrice[1] * 100) + (springWaterPrice[2]));
            int maltPrice = rhapsodyMaltCounter * ((rhapsodyMaltPrice[0] * 10000) + (rhapsodyMaltPrice[1] * 100) + (rhapsodyMaltPrice[2]));
            int footePrice = footeTripelCounter * ((footeTripelPrice[0] * 10000) + (footeTripelPrice[1] * 100) + (footeTripelPrice[2]));
            int melonPrice = melonJuiceCounter * ((melonJuicePrice[0] * 10000) + (melonJuicePrice[1] * 100) + (melonJuicePrice[2]));

            int totalCost = ribsPrice + ambrosiaPrice + lagravePrice + meadPrice + waterPrice + maltPrice + footePrice + melonPrice;

            int ribsSoldPrice = beerBastedBoarRibsSellCounter * ((beerBastedBoarRibsPrice[0] * 10000) + (beerBastedBoarRibsPrice[1] * 100) + (beerBastedBoarRibsPrice[2]));
            int ambrosiaSoldPrice = hearthglenAmbrosiaSellCounter * ((hearthglenAmbrosiaPrice[0] * 10000) + (hearthglenAmbrosiaPrice[1] * 100) + (hearthglenAmbrosiaPrice[2]));
            int lagraveSoldPrice = lagraveStoutSellCounter * ((lagraveStoutPrice[0] * 10000) + (lagraveStoutPrice[1] * 100) + (lagraveStoutPrice[2]));
            int meadSoldPrice = dwarvenMeadSellCounter * ((dwarvenMeadPrice[0] * 10000) + (dwarvenMeadPrice[1] * 100) + (dwarvenMeadPrice[2]));
            int waterSoldPrice = springWaterSellCounter * ((springWaterPrice[0] * 10000) + (springWaterPrice[1] * 100) + (springWaterPrice[2]));
            int maltSoldPrice = rhapsodyMaltSellCounter * ((rhapsodyMaltPrice[0] * 10000) + (rhapsodyMaltPrice[1] * 100) + (rhapsodyMaltPrice[2]));
            int footeSoldPrice = footeTripelSellCounter * ((footeTripelPrice[0] * 10000) + (footeTripelPrice[1] * 100) + (footeTripelPrice[2]));
            int melonSoldPrice = melonJuiceSellCounter * ((melonJuicePrice[0] * 10000) + (melonJuicePrice[1] * 100) + (melonJuicePrice[2]));

            //Return the converted prices based on the item
            if (selection.BackgroundImage == beerBastedBoarRibsDisplay.BackgroundImage)
            {
                if (sell == false)
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(ribsPrice)}";
                }
                else
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(ribsSoldPrice)}";
                }
            }
            else if (selection.BackgroundImage == melonJuiceDisplay.BackgroundImage)
            {
                if (sell == false)
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(melonPrice)}";
                }
                else
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(melonSoldPrice)}";
                }
            }
            else if (selection.BackgroundImage == legraveStoutDisplay.BackgroundImage)
            {
                if (sell == false)
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(lagravePrice)}";
                }
                else
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(lagraveSoldPrice)}";
                }
            }
            else if (selection.BackgroundImage == hearthglenAmbrosiaDisplay.BackgroundImage)
            {
                if (sell == false)
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(ambrosiaPrice)}";
                }
                else
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(ambrosiaSoldPrice)}";
                }
            }
            else if (selection.BackgroundImage == dwarvenMeadDisplay.BackgroundImage)
            {
                if (sell == false)
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(meadPrice)}";
                }
                else
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(meadSoldPrice)}";
                }
            }
            else if (selection.BackgroundImage == footeTripelDisplay.BackgroundImage)
            {
                if (sell == false)
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(footePrice)}";
                }
                else
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(footeSoldPrice)}";
                }
            }
            else if (selection.BackgroundImage == rhapsodyMaltDisplay.BackgroundImage)
            {
                if (sell == false)
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(maltPrice)}";
                }
                else
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(maltSoldPrice)}";
                }
            }
            else
            {
                if (sell == false)
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(waterPrice)}";
                }
                else
                {
                    rightSideReceiptLabel.Text += $"\n${convertPrice(waterSoldPrice)}";
                }
            }
        }

        private void TavernLoop_Tick(object sender, EventArgs e)
        {
            //Play the Background Music
            tavernMusic.Stop();
            tavernMusic.Play();
        }

        string convertPrice(Int64 priceEntered)
        {
            //Convert Sum into gold, silver, copper
            Int64 goldOrderCost = Convert.ToInt32(priceEntered / 10000);
            Int64 silverOrderCost = (Convert.ToInt32(priceEntered % 10000) - Convert.ToInt32(priceEntered % 100)) / 100;
            Int64 copperOrderCost = Convert.ToInt32(priceEntered % 100);

            string returner = $"{goldOrderCost}, {silverOrderCost}, {copperOrderCost}";
            return returner;
        }

        string inventoryItemReader(Control inventorySlot)
        {
            //Determine item and return the item's name
            if (inventorySlot.BackgroundImage == beerBastedBoarRibsDisplay.BackgroundImage)
            {
                return "Boar Ribs:";
            }
            else if (inventorySlot.BackgroundImage == springWaterDisplay.BackgroundImage)
            {
                return "Spring Water:";
            }
            else if (inventorySlot.BackgroundImage == hearthglenAmbrosiaDisplay.BackgroundImage)
            {
                return "H. Ambrosia:";
            }
            else if (inventorySlot.BackgroundImage == melonJuiceDisplay.BackgroundImage)
            {
                return "Melon Juice:";
            }
            else if (inventorySlot.BackgroundImage == footeTripelDisplay.BackgroundImage)
            {
                return "Foote Tripel:";
            }
            else if (inventorySlot.BackgroundImage == dwarvenMeadDisplay.BackgroundImage)
            {
                return "Dwarven Mead:";
            }
            else if (inventorySlot.BackgroundImage == legraveStoutDisplay.BackgroundImage)
            {
                return "Legrave Stout:";
            }
            else
            {
                return "Rhapsody Malt:";
            }
        }

        int inventoryItemLabelReader(Control inventorySlot)
        {
            //Determine the coresponding Item
            if (inventorySlot.BackgroundImage == beerBastedBoarRibsDisplay.BackgroundImage)
            {
                return beerBastedBoarRibsCounter;
            }
            else if (inventorySlot.BackgroundImage == springWaterDisplay.BackgroundImage)
            {
                return springWaterCounter;
            }
            else if (inventorySlot.BackgroundImage == hearthglenAmbrosiaDisplay.BackgroundImage)
            {
                return hearthglenAmbrosiaCounter;
            }
            else if (inventorySlot.BackgroundImage == melonJuiceDisplay.BackgroundImage)
            {
                return melonJuiceCounter;
            }
            else if (inventorySlot.BackgroundImage == footeTripelDisplay.BackgroundImage)
            {
                return footeTripelCounter;
            }
            else if (inventorySlot.BackgroundImage == dwarvenMeadDisplay.BackgroundImage)
            {
                return dwarvenMeadCounter;
            }
            else if (inventorySlot.BackgroundImage == legraveStoutDisplay.BackgroundImage)
            {
                return lagraveStoutCounter;
            }
            else
            {
                return rhapsodyMaltCounter;
            }
        }

        int itemsCheck(Control sender)
        {
            //Organize the inventory based on whether a slot is empty or if the item added is already in the slot
            if (inventoryItem1.Tag == "0" || sender.BackgroundImage == inventoryItem1.BackgroundImage)
            {
                inventoryItem1.Tag = "1";
                return 1;
            }
            else if (inventoryItem2.Tag == "0" || sender.BackgroundImage == inventoryItem2.BackgroundImage)
            {
                inventoryItem2.Tag = "1";
                return 2;
            }
            else if (inventoryItem3.Tag == "0" || sender.BackgroundImage == inventoryItem3.BackgroundImage)
            {
                inventoryItem3.Tag = "1";
                return 3;
            }
            else if (inventoryItem4.Tag == "0" || sender.BackgroundImage == inventoryItem4.BackgroundImage)
            {
                inventoryItem4.Tag = "1";
                return 4;
            }
            else if (inventoryItem5.Tag == "0" || sender.BackgroundImage == inventoryItem5.BackgroundImage)
            {
                inventoryItem5.Tag = "1";
                return 5;
            }
            else if (inventoryItem6.Tag == "0" || sender.BackgroundImage == inventoryItem6.BackgroundImage)
            {
                inventoryItem6.Tag = "1";
                return 6;
            }
            else if (inventoryItem7.Tag == "0" || sender.BackgroundImage == inventoryItem7.BackgroundImage)
            {
                inventoryItem7.Tag = "1";
                return 7;
            }
            else
            {
                inventoryItem8.Tag = "1";
                return 8;
            }
        }

        Control openSlotStatements(int slot, bool label)
        {
            //Returns the Control of the item or item label based on the slot
            if (slot == 1)
            {
                if (label == false)
                {
                    return inventoryItem1;
                }
                else
                {
                    return inventoryItemLabel1;
                }
            }
            else if (slot == 2)
            {
                if (label == false)
                {
                    return inventoryItem2;
                }
                else
                {
                    return inventoryItemLabel2;
                }
            }
            else if (slot == 3)
            {
                if (label == false)
                {
                    return inventoryItem3;
                }
                else
                {
                    return inventoryItemLabel3;
                }
            }
            else if (slot == 4)
            {
                if (label == false)
                {
                    return inventoryItem4;
                }
                else
                {
                    return inventoryItemLabel4;
                }
            }
            else if (slot == 5)
            {
                if (label == false)
                {
                    return inventoryItem5;
                }
                else
                {
                    return inventoryItemLabel5;
                }
            }
            else if (slot == 6)
            {
                if (label == false)
                {
                    return inventoryItem6;
                }
                else
                {
                    return inventoryItemLabel6;
                }
            }
            else if (slot == 7)
            {
                if (label == false)
                {
                    return inventoryItem7;
                }
                else
                {
                    return inventoryItemLabel7;
                }
            }
            else
            {
                if (label == false)
                {
                    return inventoryItem8;
                }
                else
                {
                    return inventoryItemLabel8;
                }
            }
        }

        private void itemSell(object sender, EventArgs e)
        {
            Control selectedItem;

            //Determine the selected item
            if (sender == inventoryItem1)
            {
                selectedItem = inventoryItem1;
            }
            else if (sender == inventoryItem2)
            {
                selectedItem = inventoryItem2;
            }
            else if (sender == inventoryItem3)
            {
                selectedItem = inventoryItem3;
            }
            else if (sender == inventoryItem4)
            {
                selectedItem = inventoryItem4;
            }
            else if (sender == inventoryItem5)
            {
                selectedItem = inventoryItem5;
            }
            else if (sender == inventoryItem6)
            {
                selectedItem = inventoryItem6;
            }
            else if (sender == inventoryItem7)
            {
                selectedItem = inventoryItem7;
            }
            else
            {
                selectedItem = inventoryItem8;
            }

            //If you have at least one of the item, decrease the counter and mark that an item was sold
            if (selectedItem.BackgroundImage == beerBastedBoarRibsDisplay.BackgroundImage)
            {
                int openItem = itemsCheck(beerBastedBoarRibsDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                if (beerBastedBoarRibsCounter + beerBastedBoarRibsPrevCounter >= 1)
                {
                    beerBastedBoarRibsSellCounter += 1;
                    if (beerBastedBoarRibsCounter > 0)
                    {
                        beerBastedBoarRibsCounter -= 1;
                    }
                    else
                    {
                        beerBastedBoarRibsPrevCounter -= 1;
                    }
                }
                placementSlotLabel.Text = $"{beerBastedBoarRibsCounter + beerBastedBoarRibsPrevCounter}";
            }
            else if (selectedItem.BackgroundImage == dwarvenMeadDisplay.BackgroundImage)
            {
                int openItem = itemsCheck(dwarvenMeadDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                if (dwarvenMeadCounter + dwarvenMeadPrevCounter >= 1)
                {
                    dwarvenMeadSellCounter += 1;
                    if (dwarvenMeadCounter > 0)
                    {
                        dwarvenMeadCounter -= 1;
                    }
                    else
                    {
                        dwarvenMeadPrevCounter -= 1;
                    }
                }
                placementSlotLabel.Text = $"{dwarvenMeadCounter + dwarvenMeadPrevCounter}";
            }
            else if (selectedItem.BackgroundImage == hearthglenAmbrosiaDisplay.BackgroundImage)
            {
                int openItem = itemsCheck(hearthglenAmbrosiaDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                if (hearthglenAmbrosiaCounter + hearthglenAmbrosiaPrevCounter >= 1)
                {
                    hearthglenAmbrosiaSellCounter += 1;
                    if (hearthglenAmbrosiaCounter > 0)
                    {
                        hearthglenAmbrosiaCounter -= 1;
                    }
                    else
                    {
                        hearthglenAmbrosiaPrevCounter -= 1;
                    }
                }
                placementSlotLabel.Text = $"{hearthglenAmbrosiaCounter + hearthglenAmbrosiaPrevCounter}";
            }
            else if (selectedItem.BackgroundImage == legraveStoutDisplay.BackgroundImage)
            {
                int openItem = itemsCheck(legraveStoutDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                if (lagraveStoutCounter + lagraveStoutPrevCounter >= 1)
                {
                    lagraveStoutSellCounter += 1;
                    if (lagraveStoutCounter > 0)
                    {
                        lagraveStoutCounter -= 1;
                    }
                    else
                    {
                        lagraveStoutPrevCounter -= 1;
                    }
                }
                placementSlotLabel.Text = $"{lagraveStoutCounter + lagraveStoutPrevCounter}";
            }
            else if (selectedItem.BackgroundImage == rhapsodyMaltDisplay.BackgroundImage)
            {
                int openItem = itemsCheck(rhapsodyMaltDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                if (rhapsodyMaltCounter + rhapsodyMaltPrevCounter >= 1)
                {
                    rhapsodyMaltSellCounter += 1;
                    if (rhapsodyMaltCounter > 0)
                    {
                        rhapsodyMaltCounter -= 1;
                    }
                    else
                    {
                        rhapsodyMaltPrevCounter -= 1;
                    }
                }
                placementSlotLabel.Text = $"{rhapsodyMaltCounter + rhapsodyMaltPrevCounter}";
            }
            else if (selectedItem.BackgroundImage == springWaterDisplay.BackgroundImage)
            {
                int openItem = itemsCheck(springWaterDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                if (springWaterCounter + springWaterPrevCounter >= 1)
                {
                    springWaterSellCounter += 1;
                    if (springWaterCounter > 0)
                    {
                        springWaterCounter -= 1;
                    }
                    else
                    {
                        springWaterPrevCounter -= 1;
                    }
                }
                placementSlotLabel.Text = $"{springWaterCounter + springWaterPrevCounter}";
            }
            else if (selectedItem.BackgroundImage == footeTripelDisplay.BackgroundImage)
            {
                int openItem = itemsCheck(footeTripelDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                if (footeTripelCounter + footeTripelPrevCounter >= 1)
                {
                    footeTripelSellCounter += 1;
                    if (footeTripelCounter > 0)
                    {
                        footeTripelCounter -= 1;
                    }
                    else
                    {
                        footeTripelPrevCounter -= 1;
                    }
                }
                placementSlotLabel.Text = $"{footeTripelCounter + footeTripelPrevCounter}";
            }
            else if (selectedItem.BackgroundImage == melonJuiceDisplay.BackgroundImage)
            {
                int openItem = itemsCheck(melonJuiceDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                if (melonJuiceCounter + melonJuicePrevCounter >= 1)
                {
                    melonJuiceSellCounter += 1;
                    if (melonJuiceCounter > 0)
                    {
                        melonJuiceCounter -= 1;
                    }
                    else
                    {
                        melonJuicePrevCounter -= 1;
                    }
                }
                placementSlotLabel.Text = $"{melonJuiceCounter + melonJuicePrevCounter}";
            }
        }

        private void itemClick(object sender, EventArgs e)
        {
            //Determine the sender and display the purchased item in the inventory
            if (sender == beerBastedBoarRibsDisplay)
            {
                int openItem = itemsCheck(beerBastedBoarRibsDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                beerBastedBoarRibsCounter += 1;
                placementSlot.BackgroundImage = beerBastedBoarRibsDisplay.BackgroundImage;
                placementSlotLabel.Text = $"{beerBastedBoarRibsCounter + beerBastedBoarRibsPrevCounter}";
                placementSlot.Visible = true;
                placementSlotLabel.Visible = true;
            }
            else if (sender == dwarvenMeadDisplay)
            {
                int openItem = itemsCheck(dwarvenMeadDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                dwarvenMeadCounter += 1;
                placementSlot.BackgroundImage = dwarvenMeadDisplay.BackgroundImage;
                placementSlotLabel.Text = $"{dwarvenMeadCounter + dwarvenMeadPrevCounter}";
                placementSlot.Visible = true;
                placementSlotLabel.Visible = true;
            }
            else if (sender == hearthglenAmbrosiaDisplay)
            {
                int openItem = itemsCheck(hearthglenAmbrosiaDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                hearthglenAmbrosiaCounter += 1;
                placementSlot.BackgroundImage = hearthglenAmbrosiaDisplay.BackgroundImage;
                placementSlotLabel.Text = $"{hearthglenAmbrosiaCounter + hearthglenAmbrosiaPrevCounter}";
                placementSlot.Visible = true;
                placementSlotLabel.Visible = true;
            }
            else if (sender == legraveStoutDisplay)
            {
                int openItem = itemsCheck(legraveStoutDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                lagraveStoutCounter += 1;
                placementSlot.BackgroundImage = legraveStoutDisplay.BackgroundImage;
                placementSlotLabel.Text = $"{lagraveStoutCounter + lagraveStoutPrevCounter}";
                placementSlot.Visible = true;
                placementSlotLabel.Visible = true;
            }
            else if (sender == rhapsodyMaltDisplay)
            {
                int openItem = itemsCheck(rhapsodyMaltDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                rhapsodyMaltCounter += 1;
                placementSlot.BackgroundImage = rhapsodyMaltDisplay.BackgroundImage;
                placementSlotLabel.Text = $"{rhapsodyMaltCounter + rhapsodyMaltPrevCounter}";
                placementSlot.Visible = true;
                placementSlotLabel.Visible = true;
            }
            else if (sender == springWaterDisplay)
            {
                int openItem = itemsCheck(springWaterDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                springWaterCounter += 1;
                placementSlot.BackgroundImage = springWaterDisplay.BackgroundImage;
                placementSlotLabel.Text = $"{springWaterCounter + springWaterPrevCounter}";
                placementSlot.Visible = true;
                placementSlotLabel.Visible = true;
            }
            else if (sender == footeTripelDisplay)
            {
                int openItem = itemsCheck(footeTripelDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                footeTripelCounter += 1;
                placementSlot.BackgroundImage = footeTripelDisplay.BackgroundImage;
                placementSlotLabel.Text = $"{footeTripelCounter + footeTripelPrevCounter}";
                placementSlot.Visible = true;
                placementSlotLabel.Visible = true;
            }
            else if (sender == melonJuiceDisplay)
            {
                int openItem = itemsCheck(melonJuiceDisplay);
                Control placementSlot = openSlotStatements(openItem, false);
                Control placementSlotLabel = openSlotStatements(openItem, true);
                melonJuiceCounter += 1;
                placementSlot.BackgroundImage = melonJuiceDisplay.BackgroundImage;
                placementSlotLabel.Text = $"{melonJuiceCounter + melonJuicePrevCounter}";
                placementSlot.Visible = true;
                placementSlotLabel.Visible = true;
            }
        }

        void bagHide()
        {
            //Hide the Bag
            inventoryWindow.Visible = false;
        }

        void bagShow()
        {
            //Show the Bag
            inventoryWindow.Visible = true;
        }
        private void sizeInit_Tick(object sender, EventArgs e)
        {
            //An Original Initializer. Yes it is outdated and could be moved with new knowledge, but it works
            DwarvenTavern.ActiveForm.Size = noShop;
            int backOp = rnd.Next(backgrounds.Length);
            backgroundTavern.BackgroundImage = backgrounds[backOp];
            dwarfTalkLabel.Text = "Welcome Traveller!\n     Might I interest ye in a pint...\nor perhaps an imported delight!\n     Something to clear ye head\nor warm ye belly!";
            sizeInit.Stop();
        }

        private void showShop(object sender, EventArgs e)
        {
            //Increases the screen size to show the screen
            DwarvenTavern.ActiveForm.Size = shop;
        }

        private void inventoryButtonClick(object sender, EventArgs e)
        {
            //Hides the speech window and shows the bag
            xButtonLeft_Click(sender: sender, e);
            bagShow();
        }

        private void xButtonRight_Click(object sender, EventArgs e)
        {
            //Hides the shop on click
            DwarvenTavern.ActiveForm.Size = noShop;
        }

        private void xButtonBag_Click(object sender, EventArgs e)
        {
            //Hides the bag on click
            bagHide();
        }

        private void xButtonLeft_Click(object sender, EventArgs e)
        {
            //Hides the speech window
            dwarfDisplayLeft.Visible = false;
            speechWindowNotInteractive.Visible = false;
            goodbyePress.Visible = false;
            speechLabel.Visible = false;
            xButtonLeft.Visible = false;
        }

        void dwarfName()
        {
            //Determines and displays a dwarf name
            int nameOp = rnd.Next(dwarfNames.Length);
            dwarfNameLeft.Text = dwarfNames[nameOp];
            dwarfNameRight.Text = dwarfNames[nameOp];
        }

        private void callInkeeperButton_Click(object sender, EventArgs e)
        {
            //Shows the speech window, hides the inventory, and displays the welcome text
            dwarfGreeting();
            dwarfDisplayLeft.Visible = true;
            speechWindowNotInteractive.Visible = true;
            goodbyePress.Visible = true;
            speechLabel.Visible = true;
            xButtonLeft.Visible = true;

            bagHide();

            dwarfTalkLabel.Text = "Welcome Traveller!\n     Might I interest ye in a pint...\nor perhaps an imported delight!\n     Something to clear ye head\nor warm ye belly!";
            speechLabel.Text = "Where are we?";
        }

        private void speechBubbleButton_Click(object sender, EventArgs e)
        {
            //Displays new text and hides the open shop function
            if (speechLabel.Text == "Where are we?")
            {
                dwarfTalkLabel.Text = "Why Azeroth of course! \nWhere else would we be!\n     Just sit back, relax with some \nfine ale, and don't ask too many\nquestions.";
                speechLabel.Text = "<Return>";
            }
            else if (speechLabel.Text == "<Return>")
            {
                dwarfTalkLabel.Text = "Welcome Traveller!\n     Might I interest ye in a pint...\nor perhaps an imported delight!\n     Something to clear ye head\nor warm ye belly!";
                speechLabel.Text = "Where are we?";
                leeroySpeechLabel.Text = "Leeeeroy Jennnnkins!";
            }
        }

        private void costCalculateButton_Click(object sender, EventArgs e)
        {
            //Array of Costs
            int[] footeTripelPrice = { 2, 56, 0 }, dwarvenMeadPrice = { 0, 15, 0 }, lagraveStoutPrice = { 2, 24, 0 }, springWaterPrice = { 0, 0, 25 }, beerBastedBoarRibsPrice = { 0, 10, 55 }, rhapsodyMaltPrice = { 0, 0, 50 }, hearthglenAmbrosiaPrice = { 2, 56, 0 }, melonJuicePrice = { 0, 5, 0 };

            //Determine Prices and Totals
            int ribsPrice = beerBastedBoarRibsCounter * ((beerBastedBoarRibsPrice[0] * 10000) + (beerBastedBoarRibsPrice[1] * 100) + (beerBastedBoarRibsPrice[2]));
            int ambrosiaPrice = hearthglenAmbrosiaCounter * ((hearthglenAmbrosiaPrice[0] * 10000) + (hearthglenAmbrosiaPrice[1] * 100) + (hearthglenAmbrosiaPrice[2]));
            int lagravePrice = lagraveStoutCounter * ((lagraveStoutPrice[0] * 10000) + (lagraveStoutPrice[1] * 100) + (lagraveStoutPrice[2]));
            int meadPrice = dwarvenMeadCounter * ((dwarvenMeadPrice[0] * 10000) + (dwarvenMeadPrice[1] * 100) + (dwarvenMeadPrice[2]));
            int waterPrice = springWaterCounter * ((springWaterPrice[0] * 10000) + (springWaterPrice[1] * 100) + (springWaterPrice[2]));
            int maltPrice = rhapsodyMaltCounter * ((rhapsodyMaltPrice[0] * 10000) + (rhapsodyMaltPrice[1] * 100) + (rhapsodyMaltPrice[2]));
            int footePrice = footeTripelCounter * ((footeTripelPrice[0] * 10000) + (footeTripelPrice[1] * 100) + (footeTripelPrice[2]));
            int melonPrice = melonJuiceCounter * ((melonJuicePrice[0] * 10000) + (melonJuicePrice[1] * 100) + (melonJuicePrice[2]));

            int totalCost = ribsPrice + ambrosiaPrice + lagravePrice + meadPrice + waterPrice + maltPrice + footePrice + melonPrice;

            int ribsSoldPrice = beerBastedBoarRibsSellCounter * ((beerBastedBoarRibsPrice[0] * 10000) + (beerBastedBoarRibsPrice[1] * 100) + (beerBastedBoarRibsPrice[2]));
            int ambrosiaSoldPrice = hearthglenAmbrosiaSellCounter * ((hearthglenAmbrosiaPrice[0] * 10000) + (hearthglenAmbrosiaPrice[1] * 100) + (hearthglenAmbrosiaPrice[2]));
            int lagraveSoldPrice = lagraveStoutSellCounter * ((lagraveStoutPrice[0] * 10000) + (lagraveStoutPrice[1] * 100) + (lagraveStoutPrice[2]));
            int meadSoldPrice = dwarvenMeadSellCounter * ((dwarvenMeadPrice[0] * 10000) + (dwarvenMeadPrice[1] * 100) + (dwarvenMeadPrice[2]));
            int waterSoldPrice = springWaterSellCounter * ((springWaterPrice[0] * 10000) + (springWaterPrice[1] * 100) + (springWaterPrice[2]));
            int maltSoldPrice = rhapsodyMaltSellCounter * ((rhapsodyMaltPrice[0] * 10000) + (rhapsodyMaltPrice[1] * 100) + (rhapsodyMaltPrice[2]));
            int footeSoldPrice = footeTripelSellCounter * ((footeTripelPrice[0] * 10000) + (footeTripelPrice[1] * 100) + (footeTripelPrice[2]));
            int melonSoldPrice = melonJuiceSellCounter * ((melonJuicePrice[0] * 10000) + (melonJuicePrice[1] * 100) + (melonJuicePrice[2]));

            int totalSold = ribsSoldPrice + ambrosiaSoldPrice + lagraveSoldPrice + meadSoldPrice + waterSoldPrice + maltSoldPrice + footeSoldPrice + melonSoldPrice;

            int fullCost = (totalCost - totalSold) + (Convert.ToInt32((totalCost - totalSold) * 0.13));

            //Basically the try catch statements in advance
            tenderedCorrections();

            //Converting total to a display
            goldOrderCost.Text = $"{Convert.ToInt32(fullCost / 10000)}";
            silverOrderCost.Text = $"{(Convert.ToInt32(fullCost % 10000) - Convert.ToInt32(fullCost % 100)) / 100}";
            copperOrderCost.Text = $"{Convert.ToInt32(fullCost % 100)}";

            //Gives the ability to make a receipt
            receiptButton.Enabled = true;
        }

        void dwarfGreeting()
        {
            //Play a random dwarf greeting
            string[] greetings = { "DwarfMaleGrimNPCGreeting01", "DwarfMaleGrimNPCGreeting02", "DwarfMaleGrimNPCGreeting03", "DwarfMaleGrimNPCGreeting04", "DwarfMaleGrimNPCGreeting05", "DwarfMaleGrimNPCVendor01", "DwarfMaleGrimNPCVendor02", "DwarfMaleStandardNPCGreeting01", "DwarfMaleStandardNPCGreeting02", "DwarfMaleStandardNPCGreeting03", "DwarfMaleStandardNPCGreeting04", "DwarfMaleStandardNPCGreeting05", "DwarfMaleStandardNPCGreeting06"};
            int greetOp = rnd.Next(greetings.Length);
            var dwarfGreet = new System.Windows.Media.MediaPlayer();

            dwarfGreet.Open(new Uri(Application.StartupPath + $"/Resources/{greetings[greetOp]}.wav"));

            dwarfGreet.Play();
        }
    }
}
//Easter Egg Answer: Enter 11052005 (May 11th, 2005) with the items in your inventory, then talk with the innkeeper
//Remember that items can be sold
