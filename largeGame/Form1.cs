using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace largeGame
{
    class CActor
    {
        public Rectangle rcDst;
        public Rectangle rcSrc;
        public Bitmap img;

    }
    class MActor
    {
        
        public Bitmap[] img;
        public int X, Y, W, H;
        public int frame = 0;
        public int Xdir = 1;
        public int SeeHero = 0;
        public int bulletHero = 0;
        public int deadenemy = 0;
        public int hitenemy = 0;
        public int Onshape = -1;

    }
    class SActor
    {

        public Bitmap img;
        public int X, Y, W, H;
        public int xdir = 1;
        public int ydir = -1;
        public int bulletHero = 0;
        public int xold;

    }
    public partial class Form1 : Form
    {
        Bitmap off;
        List<CActor> LWorld = new List<CActor>();
        List<MActor> LHero = new List<MActor>();
        List<SActor> LShapes = new List<SActor>();
        List<SActor> Litems = new List<SActor>();
        List<MActor> LEnemy = new List<MActor>();
        List<MActor> LSEnemy = new List<MActor>();
        List<MActor> LHbar= new List<MActor>();
        List<MActor> LSHbar = new List<MActor>();
        List<MActor> LDHbar = new List<MActor>();
        List<MActor> LAmmoBox = new List<MActor>();
        List<SActor> Lazer = new List<SActor>();
        List<SActor> LBullet= new List<SActor>();
        List<SActor> LMenu = new List<SActor>();
        List<SActor> Ludder = new List<SActor>();
        int XScroll = 0;
        int YScroll = 0;
        int heroR = 0;
        int heroL = 0;
        int ctjump = 0;
        int ctTick = 0;
        bool jump = false;
        bool jump2 = false;
        bool jump3 = false;
        bool hitshape = false;
        bool gravity = false;
        bool lazer = false;
        int ctdir = 0;
        //int ctdirE = 0;
        int elevetorDirUp = 1;
        int elevetorDirDown = -1;
        int currElevetor = -1;
        int ctSeeHero = 0;
        int cttickHit = 0;
        int ct = 0;
        int ctRemoveI = 0;
        int ctTickJumpping = 0;
        int ctElevetor = 0;
        bool canGoRight = true;
        int ctGravity = 0;
        bool CanUseLaser = false;
        bool CanUseCar = false;
        bool drawKey = true;
        bool HeroRideCar = false;
        bool ShowStart=true;
        bool ShowEnd=false;
        bool ShowWinner = false;
        bool GameInProccess=false;
        bool HeroJumpping=false;
        Timer tt = new Timer();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            tt.Tick += Tt_Tick;
            
        }

        private void Tt_Tick(object sender, EventArgs e)
        {

            //clear bullets that moves 200 x axis
            for(int i=0;i<LBullet.Count;i++)
            {
                int difference = LBullet[i].X - LBullet[i].xold;

                if(difference<0)
                {
                    difference *= -1;
                }

                if(difference>=250)
                {
                    LBullet.RemoveAt(i);
                }
            }
            // End of game when winning
            if (Litems[1].X > (LWorld[0].img.Width - ClientSize.Width) && HeroRideCar == true)
            {
                ShowWinner=true;
                DrawDubb(this.CreateGraphics());
                tt.Stop();
            }
                //Car move out of screen Hero Win 
            if (HeroRideCar==true)
            {
                Litems[1].X += 20;
                

            }

            //Take The Car Key
            if (LHero[0].X < Litems[0].X && LHero[0].X + LHero[0].W > Litems[0].X && LHero[0].Y < Litems[0].Y && LHero[0].Y + LHero[0].H > Litems[0].Y)
            {
                CanUseCar = true;
                drawKey = false;
            }

            //clear open ammo boxes
            if (ctTick % 20 == 0)
            {
                for (int i = 0; i < LAmmoBox.Count; i++)
                {
                    if (LAmmoBox[i].frame > 0)
                    {
                        LAmmoBox.RemoveAt(i);

                    }
                }
            }

            //move elevetor
            if(ctTick % 60 == 0)
            {
                ctElevetor++;
                if (ctElevetor % 2 == 0)
                {
                    LShapes[5].ydir = 1;
                }
                else
                {
                    LShapes[5].ydir = -1;
                }
            }
            LShapes[5].Y += 10 * LShapes[5].ydir;
            //move hero with elevetor
            if (LHero[0].Onshape==5 && HeroJumpping==false)
            {
                LHero[0].Y+= 10 * LShapes[5].ydir;

            }
            //check jump
            if (jump==true)
            {
                ctTickJumpping++;
                LHero[0].Y -= 15;
                HeroJumpping = true;
                if (ctTickJumpping <=6)
                {
                   

                }
                else
                {
                    jump = false;
                    gravity = true;
                    
                }
            }
            if (jump2 == true)
            {
                ctTickJumpping++;
                LHero[0].Y -= 15;
                LHero[0].X += 15;
                HeroJumpping = true;
                if (ctTickJumpping <= 6)
                {


                }
                else
                {
                    jump2 = false;
                    gravity = true;

                }
            }

            if (jump3 == true)
            {
                ctTickJumpping++;
                LHero[0].Y -= 15;
                LHero[0].X -= 15;
                HeroJumpping = true;
                if (ctTickJumpping <= 6)
                {


                }
                else
                {
                    jump3 = false;
                    gravity = true;

                }
            }
            //check gravitiy of hero 
            if (jump==false && gravity==true)
            {
               

                if (LHero[0].Y >= this.Height-96-50)
                {
                    HeroJumpping = false;
                    gravity = false;
                    LHero[0].Onshape = -1;
                    ctGravity++;
                }
                
               
                for (int i=0;i<LShapes.Count;i++)
                {
                    if (LHero[0].Y + LHero[0].H >= LShapes[i].Y&& LHero[0].Y + LHero[0].H < LShapes[i].Y+50 && LHero[0].X >= LShapes[i].X && LHero[0].X < LShapes[i].X + LShapes[i].W )
                    {
                        //this.Text = "sec";
                        HeroJumpping = false;
                        gravity = false;
                        LHero[0].Onshape = i;
                        ctGravity++;
                    }
                    
                }
                
                
                if(ctGravity==0)
                {
                    LHero[0].Y += 15;
                    
                    gravity = true;
                    HeroJumpping = true;
                }
                else
                {
                    ctGravity = 0;
                }
                

                
            }



            
            //check bullet hero hit double enemy or single enemy 
            
           
                for (int i = 0; i < LBullet.Count; i++)
                {
                    for (int k = 0; k < LEnemy.Count; k++)
                    {
                        if (LBullet[i].X >= LEnemy[k].X && LBullet[i].X < LEnemy[k].X + LEnemy[k].W && LBullet[i].Y > LEnemy[k].Y && LBullet[i].Y < LEnemy[k].Y + LEnemy[k].H && LBullet[i].bulletHero == 1 && LEnemy[k].deadenemy==0)
                        {
                             //LBullet.RemoveAt(i);
                             ctRemoveI++;
                            LEnemy[k].hitenemy++;
                            if (LEnemy[k].hitenemy < 4)
                            {
                                LDHbar[k].frame = LEnemy[k].hitenemy;
                            }
                                if (LEnemy[k].hitenemy>2)
                                {

                                    
                                        if (LEnemy[k].frame % 2 == 0)
                                        {
                                            LEnemy[k].frame = 9;
                                        }
                                        else
                                        {
                                            LEnemy[k].frame = 8;
                                        }
                                    
                                    
                                    LEnemy[k].deadenemy = 1;
                                    LEnemy[k].Y += LEnemy[k].H;
                                    LHbar[0].frame--;
                                    
                                    if (LHbar[0].frame   <= 0)
                                    {
                                         LHbar[0].frame = 0;
                                    }
                                    //LEnemy.RemoveAt(k);
                                }
                            break;
                           

                        }
                        
                    }
                     //BULLET hit  single enemy
                    for (int k = 0; k < LSEnemy.Count; k++)
                    {
                        if (LBullet[i].X >= LSEnemy[k].X && LBullet[i].X < LSEnemy[k].X + LSEnemy[k].W && LBullet[i].Y > LSEnemy[k].Y && LBullet[i].Y < LSEnemy[k].Y + LSEnemy[k].H && LBullet[i].bulletHero == 1 && LSEnemy[k].deadenemy == 0)
                        {
                            
                            ctRemoveI++;
                            LSEnemy[k].hitenemy++;
                            if (LSEnemy[k].hitenemy < 4)
                            {
                                LSHbar[k].frame = LSEnemy[k].hitenemy;
                            }
                            if (LSEnemy[k].hitenemy > 2)
                            {
                               
                                if (LSEnemy[k].frame % 2 == 0)
                                {
                                        LSEnemy[k].frame = 9;
                                }
                                else
                                {
                                        LSEnemy[k].frame = 8;
                                }
                                    LSEnemy[k].deadenemy = 1;
                                    LSEnemy[k].Y += LSEnemy[k].H;
                                LHbar[0].frame--;
                               
                                if (LHbar[0].frame <= 3)
                                {
                                    LHbar[0].frame = 0;
                                }
                                 //LEnemy.RemoveAt(k);
                            }
                            break;


                        }

                    }
                     //Bullet hit hero 
                    if (LBullet[i].X >= LHero[0].X && LBullet[i].X < LHero[0].X + LHero[0].W && LBullet[i].Y > LHero[0].Y && LBullet[i].Y < LHero[0].Y + LHero[0].H && LBullet[i].bulletHero == 0 && LHero[0].deadenemy == 0)
                    {

                        ctRemoveI++;
                        LHero[0].hitenemy= LHbar[0].frame +1;
                        if (LHero[0].hitenemy<4)
                        {
                            LHbar[0].frame = LHero[0].hitenemy;
                        }
                        if (LHero[0].hitenemy > 2)
                        {
                              ShowEnd = true;
                              
                            LHero[0].deadenemy = 1;
                                tt.Stop();

                        }
                        
                      


                    }
                   
                    if (ctRemoveI==1)
                    {
                        LBullet.RemoveAt(i);
                        ctRemoveI = 0;
                    }
                }

                //check laser hero hit double enemy or single enemy and check if soldier lazer hit hero
                for(int i=0;i<Lazer.Count;i++)
                {
                    for (int k = 0; k < LEnemy.Count; k++)
                    {
                        if (Lazer[i].X+ Lazer[i].W >= LEnemy[k].X && Lazer[i].X < LEnemy[k].X + LEnemy[k].W && Lazer[i].Y > LEnemy[k].Y && Lazer[i].Y < LEnemy[k].Y + LEnemy[k].H  && LEnemy[k].deadenemy == 0 && Lazer[i].bulletHero==1)
                        {
                            

                            LEnemy[k].hitenemy+=3;
                            if (LEnemy[k].hitenemy > 2)
                            {
                                if (LEnemy[k].frame % 2 == 0)
                                {
                                    LEnemy[k].frame = 9;
                                    LDHbar[k].frame = 3;
                                }
                                else
                                {
                                        LEnemy[k].frame = 8;
                                    LDHbar[k].frame = 3;
                                }
                                LEnemy[k].deadenemy = 1;
                                LEnemy[k].Y += LEnemy[k].H;
                                LHbar[0].frame--;
                                
                                if (LHbar[0].frame <=0)
                                {
                                    LHbar[0].frame = 0;
                                }
                                 //LEnemy.RemoveAt(k);
                            }
                            break;


                        }

                        


                    }
                    //lazer hit hero
                    if (Lazer[i].X + Lazer[i].W >= LHero[0].X && Lazer[i].X < LHero[0].X + LHero[0].W && Lazer[i].Y > LHero[0].Y && Lazer[i].Y < LHero[0].Y + LHero[0].H && LHero[0].deadenemy == 0 && Lazer[i].bulletHero == 0)
                    {
                         LHero[0].hitenemy = LHbar[0].frame + 2; 
                        if (LHero[0].hitenemy <= 2)
                        {
                            LHbar[0].frame = LHero[0].hitenemy;


                            //LEnemy.RemoveAt(k);
                        }
                        else
                        {
                            LHbar[0].frame = 3;
                            LHero[0].deadenemy = 1;
                              ShowEnd = true;

                            
                            tt.Stop();
                        }
                        break;
                    }
                      //lazer hit  single enemy
                    for (int k = 0; k < LSEnemy.Count; k++)
                    {
                            if (Lazer[i].X + Lazer[i].W >= LSEnemy[k].X && Lazer[i].X < LSEnemy[k].X + LSEnemy[k].W && Lazer[i].Y > LSEnemy[k].Y && Lazer[i].Y < LSEnemy[k].Y + LSEnemy[k].H && LSEnemy[k].deadenemy == 0)
                            {


                                LSEnemy[k].hitenemy += 3;
                                if (LSEnemy[k].hitenemy >2)
                                {
                                    if (LSEnemy[k].frame % 2 == 0)
                                    {
                                         LSEnemy[k].frame = 9;
                                        LSHbar[k].frame = 3;
                                    }
                                    else
                                    {
                                        LSEnemy[k].frame = 8;
                                          LSHbar[k].frame = 3;
                                    }
                                    LSEnemy[k].deadenemy = 1;
                                    LSEnemy[k].Y += LSEnemy[k].H;
                                    LHbar[0].frame--;
                                    
                                    if (LHbar[0].frame <= 0)
                                    {
                                        LHbar[0].frame = 0;
                                    }

                                    //LEnemy.RemoveAt(k);
                                }
                                break;


                            }

                    }
                }


                // fall double enemy and single 
                if (ctTick % 3 ==0)
                {
                          for(int i=0;i< LEnemy.Count;i++)
                          {
                            if (LEnemy[i].deadenemy==1 && LEnemy[i].Y <=this.Height - 96 - 50)
                            {
                                LEnemy[i].Y += 20;
                               
                            }
                            else
                            {
                                if (i == 4 && LEnemy[i].deadenemy == 1)
                                {
                                    if(LEnemy[i].frame % 2==0)
                                    {
                                          LEnemy[i].frame = 10;
                                    }
                                    else
                                    {
                                        LEnemy[i].frame = 11;
                                    }
                                }
                            }
                    
                          }

                    for (int i = 0; i < LSEnemy.Count; i++)
                    {
                        if (LSEnemy[i].deadenemy == 1 && LSEnemy[i].Y <= this.Height - 96 - 50)
                        {
                            LSEnemy[i].Y += 20;
                        }
                    }
                }

           
            if(ctTick % 1==0)
            {
                //enemy[4]
                if (LHero[0].X > LEnemy[4].X && LHero[0].X < LEnemy[4].X + LEnemy[4].W + 200 && LHero[0].Y < LEnemy[4].Y + LEnemy[4].H && LHero[0].Y + LHero[0].H > LEnemy[4].Y && LEnemy[4].deadenemy == 0)
                {
                    LEnemy[4].SeeHero = 1;
                    LEnemy[4].frame = 0;
                    CreateLazer(LEnemy[4].X, LEnemy[4].Y, 1, 0);
                    ctSeeHero++;
                }

                if (LHero[0].X < LEnemy[4].X && LHero[0].X > LEnemy[4].X - 200 && LHero[0].Y < LEnemy[4].Y + LEnemy[4].H && LHero[0].Y + LHero[0].H > LEnemy[4].Y && LEnemy[4].deadenemy == 0)
                {
                    LEnemy[4].SeeHero = 1;
                    LEnemy[4].frame = 1;
                    CreateLazer(LEnemy[4].X, LEnemy[4].Y, -1, 0);
                    ctSeeHero++;
                }
                //enemy[5]
                if (LHero[0].X > LEnemy[5].X && LHero[0].X < LEnemy[5].X + LEnemy[5].W + 200 && LHero[0].Y < LEnemy[5].Y + LEnemy[5].H && LHero[0].Y + LHero[0].H > LEnemy[5].Y && LEnemy[5].deadenemy == 0)
                {
                    LEnemy[5].SeeHero = 1;
                    LEnemy[5].frame = 0;
                    CreateLazer(LEnemy[5].X, LEnemy[5].Y, 1, 0);
                    ctSeeHero++;
                }

                if (LHero[0].X < LEnemy[5].X && LHero[0].X > LEnemy[5].X - 200 && LHero[0].Y < LEnemy[5].Y + LEnemy[5].H && LHero[0].Y + LHero[0].H > LEnemy[5].Y && LEnemy[5].deadenemy == 0)
                {
                    LEnemy[5].SeeHero = 1;
                    LEnemy[5].frame = 1;
                    CreateLazer(LEnemy[5].X, LEnemy[5].Y, -1, 0);
                    ctSeeHero++;
                }
            }
            //Double enemy Check Hero
            if (ctTick % 5 == 0)
            {
                for (int i = 0; i < LEnemy.Count-2; i++)
                {
                    if (i != 4 || i !=5 )
                    {
                        if (LHero[0].X > LEnemy[i].X && LHero[0].X < LEnemy[i].X + LEnemy[i].W + 200 && LHero[0].Y < LEnemy[i].Y + LEnemy[i].H && LHero[0].Y + LHero[0].H > LEnemy[i].Y && LEnemy[i].deadenemy == 0)
                        {
                            LEnemy[i].SeeHero = 1;
                            LEnemy[i].frame = 0;
                            CreateBullet(LEnemy[i].X, LEnemy[i].Y, 1, 0);
                            ctSeeHero++;
                        }

                        if (LHero[0].X < LEnemy[i].X && LHero[0].X > LEnemy[i].X - 200 && LHero[0].Y < LEnemy[i].Y + LEnemy[i].H && LHero[0].Y + LHero[0].H > LEnemy[i].Y && LEnemy[i].deadenemy == 0)
                        {
                            LEnemy[i].SeeHero = 1;
                            LEnemy[i].frame = 1;
                            CreateBullet(LEnemy[i].X, LEnemy[i].Y, -1, 0);
                            ctSeeHero++;
                        }
                    }
                    else
                    {
                        /*
                        if (LHero[0].X > LEnemy[i].X && LHero[0].X < LEnemy[i].X + LEnemy[i].W + 200 && LHero[0].Y < LEnemy[i].Y + LEnemy[i].H && LHero[0].Y + LHero[0].H > LEnemy[i].Y && LEnemy[i].deadenemy == 0)
                        {
                            LEnemy[i].SeeHero = 1;
                            LEnemy[i].frame = 0;
                            CreateLazer(LEnemy[i].X, LEnemy[i].Y, 1, 0);
                            ctSeeHero++;
                        }

                        if (LHero[0].X < LEnemy[i].X && LHero[0].X > LEnemy[i].X - 200 && LHero[0].Y < LEnemy[i].Y + LEnemy[i].H && LHero[0].Y + LHero[0].H > LEnemy[i].Y && LEnemy[i].deadenemy == 0)
                        {
                            LEnemy[i].SeeHero = 1;
                            LEnemy[i].frame = 1;
                            CreateLazer(LEnemy[i].X, LEnemy[i].Y, -1, 0);
                            ctSeeHero++;
                        }
                        */
                    }
                }

                //Single enemy Check Hero
                for (int i = 0; i < LSEnemy.Count; i++)
                {
                    if (LHero[0].X > LSEnemy[i].X && LHero[0].X < LSEnemy[i].X + LSEnemy[i].W + 220 && LHero[0].Y < LSEnemy[i].Y + LSEnemy[i].H && LHero[0].Y + LHero[0].H > LSEnemy[i].Y && LSEnemy[i].deadenemy == 0)
                    {
                        LSEnemy[i].SeeHero = 1;
                        LSEnemy[i].frame = 0;
                        CreateBullet(LSEnemy[i].X, LSEnemy[i].Y, 1, 0);
                        ctSeeHero++;
                    }

                    if (LHero[0].X < LSEnemy[i].X && LHero[0].X > LSEnemy[i].X - 220 && LHero[0].Y < LSEnemy[i].Y + LSEnemy[i].H && LHero[0].Y + LHero[0].H > LSEnemy[i].Y && LSEnemy[i].deadenemy == 0)
                    {
                        LSEnemy[i].SeeHero = 1;
                        LSEnemy[i].frame = 1;
                        CreateBullet(LSEnemy[i].X, LSEnemy[i].Y, -1, 0);
                        ctSeeHero++;
                    }
                }

                if (ctSeeHero == 0)
                {
                    for (int i = 0; i < LEnemy.Count; i++)
                    {
                        LEnemy[i].SeeHero = 0;
                        
                    }
                    for (int i = 0; i < LSEnemy.Count; i++)
                    {
                        LSEnemy[i].SeeHero = 0;

                    }
                    
                }
                else
                {
                    ctSeeHero = 0;
                }
            }
            

            //move bullet
            for (int i = 0; i < LBullet.Count; i++)
            {
                LBullet[i].X += 25 * LBullet[i].xdir;
            }

            //remove lazer after 3 sec 
            if(ctTick % 5 == 0 )
            {
                if(lazer == true)
                {
                    Lazer = new List<SActor>();
                    lazer = false;
                }

            }

            //move enemy
            for(int i=0;i<LEnemy.Count;i++)
            {
                
                
                
                if (LEnemy[i].SeeHero == 0 && LEnemy[i].deadenemy == 0)
                {
                   
                    if (i == 0 || i == 1)
                    {
                        LEnemy[i].X += 5 * LEnemy[i].Xdir;
                        
                        if (LEnemy[i].X + LEnemy[i].W >= LShapes[0].X + LShapes[0].W)

                        {
                            
                            LEnemy[i].Xdir = -1;
                            LEnemy[i].frame = 1;

                          

                           

                        }
                        if (LEnemy[i].X <= LShapes[0].X)
                        {
                            LEnemy[i].Xdir = 1;
                            LEnemy[i].frame = 0;

                            

                           
                        }
                        
                        if (LEnemy[i].frame == 6)
                        {
                            LEnemy[i].frame = 0;
                        }
                        if (LEnemy[i].frame == 7)
                        {
                            LEnemy[i].frame = 1;
                        }
                        LEnemy[i].frame += 2;
                    }
                }
                if (LEnemy[i].SeeHero == 0 && LEnemy[i].deadenemy==0 )
                {
                   
                    if (i == 2 || i == 3)
                    {
                        LEnemy[i].X += 5 * LEnemy[i].Xdir;
                        if (LEnemy[i].X + LEnemy[i].W >= LShapes[3].X + LShapes[3].W)
                        {
                            LEnemy[i].Xdir = -1;
                            LEnemy[i].frame = 1;
                        }
                        if (LEnemy[i].X <= LShapes[3].X)
                        {
                            LEnemy[i].Xdir = 1;
                            LEnemy[i].frame = 0;
                        }
                        if (LEnemy[i].frame == 6)
                        {
                            LEnemy[i].frame = 0;
                        }
                        if (LEnemy[i].frame == 7)
                        {
                            LEnemy[i].frame = 1;
                        }
                        LEnemy[i].frame += 2;
                    }
                }
                //new 
                if (LEnemy[i].SeeHero == 0 && LEnemy[i].deadenemy == 0)
                {

                    if (i == 4 || i==5 )
                    {
                        LEnemy[i].frame += 2;
                        LEnemy[i].X += 5 * LEnemy[i].Xdir;
                        if (LEnemy[i].X + LEnemy[i].W >= LShapes[6].X + LShapes[6].W)
                        {
                            LEnemy[i].Xdir = -1;
                            LEnemy[i].frame = 1;
                        }
                        if (LEnemy[i].X <= LShapes[6].X)
                        {
                            LEnemy[i].Xdir = 1;
                            LEnemy[i].frame = 0;
                        }
                        if (LEnemy[i].frame == 6)
                        {
                            LEnemy[i].frame = 0;
                        }
                        if (LEnemy[i].frame == 7)
                        {
                            LEnemy[i].frame = 1;
                        }
                       
                    }
                }
            }
            
                //change signle enemy dir every 10 tick
                if (ctTick % 10 == 0)
                {
                    for (int i = 0; i < LSEnemy.Count; i++)
                    {
                        if (LSEnemy[i].SeeHero == 0 && LSEnemy[i].deadenemy == 0)
                        {
                            ctdir++; 
                            if ( ctdir%2== 0)
                            {

                                 LSEnemy[i].frame = 0;
                            }
                            else
                            {
                                LSEnemy[i].frame = 1;
                            }
                        }
                    }
                }
                //move Health bar
                for(int i=0;i<LHbar.Count;i++)
                {
                    
                        LHbar[i].X = LHero[0].X;
                         LHbar[i].Y = LHero[0].Y - LHbar[i].H;
                    
                }

                for(int i=0;i<LSHbar.Count;i++)
                {
                    LSHbar[i].X = LSEnemy[i].X;
                     LSHbar[i].Y = LSEnemy[i].Y - LSHbar[i].H;
                }


            for (int i = 0; i < LDHbar.Count; i++)
            {
                LDHbar[i].X = LEnemy[i].X;
                LDHbar[i].Y = LEnemy[i].Y - LDHbar[i].H;
            }


            //this.Text = LHero[0].X + " value;";
            ctTick++;
            ModifyRects();
            DrawDubb(this.CreateGraphics());
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (GameInProccess == true)
            {
                if (e.KeyCode == Keys.Right)
                {
                    for (int k = 0; k < LShapes.Count; k++)
                    {

                        if (LHero[0].X + 56 >= LShapes[k].X && LHero[0].Y < LShapes[k].Y && LHero[0].Y + LHero[0].H > LShapes[k].Y)
                        {
                            //canGoRight = false;
                        }
                        /*
                        if (LHero[0].X + 56 >= LShapes[k].X && LHero[0].Y < LShapes[k].Y && LHero[0].Y + LHero[0].H > LShapes[k].Y +100)
                        {
                            canGoRight = false;
                        }
                        */
                    }
                    if (canGoRight == true)
                    {
                        gravity = true;
                        if (XScroll + 10 <= (LWorld[0].img.Width - ClientSize.Width))
                        {
                            XScroll += 10;
                            for (int i = 0; i < LShapes.Count; i++)
                            {
                                LShapes[i].X -= 10;

                            }

                            for (int i = 0; i < LEnemy.Count; i++)
                            {
                                LEnemy[i].X -= 10;
                            }

                            for (int i = 0; i < LSEnemy.Count; i++)
                            {
                                LSEnemy[i].X -= 10;
                            }

                            for (int i = 0; i < Ludder.Count; i++)
                            {
                                Ludder[i].X -= 10;
                            }

                            for (int i = 0; i < LAmmoBox.Count; i++)
                            {
                                LAmmoBox[i].X -= 10;
                            }


                            for (int i = 0; i < Litems.Count; i++)
                            {
                                Litems[i].X -= 10;
                            }

                            LHero[0].X += 5;

                        }


                        if (heroR == 0)
                        {
                            heroR++;
                            LHero[0].frame = 0;
                            heroL = 0;
                        }
                        if (LHero[0].frame == 8)
                        {
                            LHero[0].frame = 0;
                        }


                        if (LHero[0].frame == 6)
                        {
                            LHero[0].frame = 2;
                        }
                        else
                        {
                            LHero[0].frame += 2;
                        }
                    }
                    else
                    {
                        canGoRight = true;
                    }




                }



                if (e.KeyCode == Keys.Left)
                {

                    gravity = true;
                    if (XScroll - 10 >= 0)
                    {
                        XScroll -= 10;

                        for (int i = 0; i < LShapes.Count; i++)
                        {
                            LShapes[i].X += 10;
                        }

                        for (int i = 0; i < LEnemy.Count; i++)
                        {
                            LEnemy[i].X += 10;
                        }

                        for (int i = 0; i < LSEnemy.Count; i++)
                        {
                            LSEnemy[i].X += 10;
                        }

                        for (int i = 0; i < Ludder.Count; i++)
                        {
                            Ludder[i].X += 10;
                        }

                        for (int i = 0; i < LAmmoBox.Count; i++)
                        {
                            LAmmoBox[i].X += 10;
                        }

                        for (int i = 0; i < Litems.Count; i++)
                        {
                            Litems[i].X += 10;
                        }
                        LHero[0].X -= 5;

                    }

                    if (heroL == 0)
                    {
                        heroL++;

                        heroR = 0;

                        LHero[0].frame = 1;
                    }

                    if (LHero[0].frame == 9)
                    {
                        LHero[0].frame = 1;
                    }

                    LHero[0].frame += 2;

                    if (LHero[0].frame == 7)
                    {
                        LHero[0].frame = 1;
                    }



                }
                if (e.KeyCode == Keys.Space)
                {
                    if (HeroJumpping == false)
                    {
                        ctTickJumpping = 0;
                        jump = true;
                    }

                }

                if (e.KeyCode == Keys.D)
                {
                    
                    if (HeroJumpping == false)
                    {
                        ctTickJumpping = 0;
                        jump2 = true;
                    }

                }

                if (e.KeyCode == Keys.A)
                {
                    
                    if (HeroJumpping == false)
                    {
                        ctTickJumpping = 0;
                        jump3 = true;
                    }

                }


                if (e.KeyCode == Keys.Up)
                {
                    for (int i = 0; i < Ludder.Count; i++)
                    {
                        if (LHero[0].X > Ludder[i].X && LHero[0].X + LHero[0].W < Ludder[i].X + Ludder[i].W
                            && LHero[0].Y + LHero[0].H > Ludder[i].Y)
                        {
                            LHero[0].Y -= 20;
                            gravity = false;
                        }
                    }

                }

                if (e.KeyCode == Keys.Down)
                {
                    for (int i = 0; i < Ludder.Count; i++)
                    {
                        if (LHero[0].X > Ludder[i].X && LHero[0].X + LHero[0].W < Ludder[i].X + Ludder[i].W && LHero[0].Y <= this.Height - 96 - 50)

                        {
                            LHero[0].Y += 20;
                            gravity = false;
                        }
                    }

                }

                if (e.KeyCode == Keys.Q)
                {
                    if (LHero[0].frame % 2 == 0)
                    {
                        CreateBullet(LHero[0].X, LHero[0].Y, 1, 1);
                        LHero[0].frame = 8;
                    }
                    else
                    {
                        CreateBullet(LHero[0].X, LHero[0].Y, -1, 1);
                        LHero[0].frame = 9;
                    }

                }


                if (e.KeyCode == Keys.W)
                {
                    if (CanUseLaser == true)
                    {
                        if (LHero[0].frame % 2 == 0)
                        {
                            CreateLazer(LHero[0].X, LHero[0].Y, 1, 1);
                            LHero[0].frame = 8;
                        }
                        else
                        {
                            CreateLazer(LHero[0].X, LHero[0].Y, -1, 1);
                            LHero[0].frame = 9;
                        }
                    }

                }


                if (e.KeyCode == Keys.R)
                {
                    for (int i = 0; i < LAmmoBox.Count; i++)
                    {
                        if (i ==0 || i==2)
                        {
                            if (LAmmoBox[i].Y > LHero[0].Y && LAmmoBox[i].Y + LAmmoBox[i].H < LHero[0].Y + LHero[0].H && LHero[0].X < LAmmoBox[i].X && LHero[0].X + LHero[0].W < LAmmoBox[i].X + LAmmoBox[i].W && LAmmoBox[i].frame == 0)
                            {
                                LAmmoBox[i].frame = 1;
                            }
                        }
                        if(i==1)
                        {
                            if (LAmmoBox[1].Y > LHero[0].Y && LAmmoBox[1].Y + LAmmoBox[1].H < LHero[0].Y + LHero[0].H && LHero[0].X < LAmmoBox[1].X && LHero[0].X + LHero[0].W > LAmmoBox[1].X  && LAmmoBox[1].frame == 0 )
                            {
                                LAmmoBox[1].frame = 2;
                                CanUseLaser = true;

                            }
                        }
                    }
                }

                if (e.KeyCode == Keys.F)
                {
                    if (CanUseCar == true)
                    {
                        if (LHero[0].X > Litems[1].X && LHero[0].X + LHero[0].W < Litems[1].X + Litems[1].W && LHero[0].Y > Litems[1].Y && LHero[0].Y + LHero[0].H > Litems[1].Y)
                        {
                            HeroRideCar = true;
                        }
                    }
                }

                if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }

               
            }

            if (e.KeyCode == Keys.Enter)
            {
                if (ShowStart == true || ShowWinner == true || ShowEnd == true)
                {
                    XScroll = 0;
                    XScroll = 0;
                    YScroll = 0;
                    heroR = 0;
                    heroL = 0;
                    ctjump = 0;
                    ctTick = 0;
                    jump = false;
                    hitshape = false;
                    gravity = false;
                    lazer = false;
                    ctdir = 0;
                    //ctdirE = 0;
                    elevetorDirUp = 1;
                    elevetorDirDown = -1;
                    currElevetor = -1;
                    ctSeeHero = 0;
                    cttickHit = 0;
                    ct = 0;
                    ctRemoveI = 0;
                    ctTickJumpping = 0;
                    ctElevetor = 0;
                    canGoRight = true;
                    ctGravity = 0;
                    CanUseLaser = false;
                    CanUseCar = false;
                    drawKey = true;
                    HeroRideCar = false;
                    LWorld = new List<CActor>();
                    LHero = new List<MActor>();
                    LShapes = new List<SActor>();
                    Litems = new List<SActor>();
                    LEnemy = new List<MActor>();
                    LSEnemy = new List<MActor>();
                    LHbar = new List<MActor>();
                    LSHbar = new List<MActor>();
                    LDHbar = new List<MActor>();
                    LAmmoBox = new List<MActor>();
                    Lazer = new List<SActor>();
                    LBullet = new List<SActor>();
                    //LMenu = new List<SActor>();
                    Ludder = new List<SActor>();
                    CreateWorld();
                    CreateHero();
                    CreateShapes();
                    CreateDoubleEnemy();
                    CreateSingleEnemy();
                    CreateLudder();
                    CreateHeroHealthBar();
                    CreateSingleHealthBar();
                    CreateDoubleHealthBar();
                    CreateAmmoBox();

                    this.Text = ctTick + "   " + ctjump;
                    ShowStart = false;
                    ShowEnd = false;
                    ShowWinner = false;
                    GameInProccess = true;
                    tt.Start();
                    //DrawDubb(this.CreateGraphics());
                }
            }
            if (GameInProccess==true)
            {
                ModifyRects();
            }
            
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
           
            CreateMenus();

            DrawDubb(this.CreateGraphics());
        }

        void CreateMenus()
        {
            SActor pnn = new SActor();
            pnn.img = new Bitmap("start.jpg");
            pnn.X =0;
            pnn.Y =0;
            pnn.W = this.Width;
            pnn.H = this.Height;

            LMenu.Add(pnn);

             pnn = new SActor();
            pnn.img = new Bitmap("end.jpg");
            pnn.X = 0;
            pnn.Y = 0;
            pnn.W = this.Width;
            pnn.H = this.Height;

            LMenu.Add(pnn);

            pnn = new SActor();
            pnn.img = new Bitmap("winner.png");
            pnn.X = 0;
            pnn.Y = 0;
            pnn.W = this.Width;
            pnn.H = this.Height;

            LMenu.Add(pnn);
        }

        void CreateAmmoBox()
        {
            
            Random RR = new Random();
             

            for(int k=0;k<3;k++)
            {
                MActor pnn = new MActor();
                pnn.img = new Bitmap[3];
                int v = RR.Next(0, 2);
               
                if(k==0)
                {
                    v = 1;
                }
                if(k==1)
                {
                    v = 3;
                }

                if(k==2)
                {
                    v = 0;
                }
                pnn.X = LShapes[v].X + LShapes[v].W -30;
                for (int i = 0; i < 3; i++)
                {
                    string ss = "ammobox" + (i + 1) + ".png";
                    pnn.img[i] = new Bitmap(ss);
                    pnn.W = 30;
                    pnn.H = 20;
                }

                
                pnn.frame = 0;
                pnn.Y = LShapes[v].Y - pnn.H;


                LAmmoBox.Add(pnn);
            }

        }
        void CreateSingleHealthBar()
        {
            MActor pnn = new MActor();
            for (int i = 0; i < LSEnemy.Count; i++)
            {
                pnn = new MActor();
                pnn.img = new Bitmap[4];
                pnn.X = LSEnemy[i].X;
                for (int k = 0; k < 4; k++)
                {
                    string ss = "health" + (k + 1) + ".png";
                    pnn.img[k] = new Bitmap(ss);
                    pnn.W = 50;
                    pnn.H = 10;
                }

                pnn.Xdir = 1;
                pnn.frame = 0;
                pnn.Y = LSEnemy[i].Y - pnn.H;


                LSHbar.Add(pnn);
            }
        }
        void CreateDoubleHealthBar()
        {
            MActor pnn = new MActor();
            for (int i = 0; i < LEnemy.Count; i++)
            {
                pnn = new MActor();
                pnn.img = new Bitmap[4];
                pnn.X = LEnemy[i].X;
                for (int k = 0; k < 4; k++)
                {
                    string ss = "health" + (k + 1) + ".png";
                    pnn.img[k] = new Bitmap(ss);
                    pnn.W = 50;
                    pnn.H = 10;
                }

                pnn.Xdir = 1;
                pnn.frame = 0;
                pnn.Y = LEnemy[i].Y - pnn.H;


                LDHbar.Add(pnn);
            }
        }
        void CreateHeroHealthBar()
        {
            MActor pnn = new MActor();
            
           

             pnn = new MActor();
            pnn.img = new Bitmap[4];
            pnn.X = LHero[0].X;
            for (int k = 0; k < 4; k++)
            {
                string ss = "health" + (k + 1) + ".png";
                pnn.img[k] = new Bitmap(ss);
                pnn.W = 50;
                pnn.H = 10;
            }

            pnn.Xdir = 1;
            pnn.frame = 0;
            pnn.Y = LHero[0].Y - pnn.H;


            LHbar.Add(pnn);

        }

        void CreateDoubleEnemy()
        {
            //enemy in shape[0]
            MActor pnn = new MActor();
            pnn.img = new Bitmap[10];
            pnn.X = LShapes[0].X;
            for (int i = 0; i < 10; i++)
            {
                string ss = "e" + (i + 1) + ".png";
                pnn.img[i] = new Bitmap(ss);
                pnn.W = 56;
                pnn.H =96;
            }

            pnn.Xdir = 1;
            pnn.frame = 0;
            pnn.Y = LShapes[0].Y-pnn.H;

            
            LEnemy.Add(pnn);

            pnn = new MActor();
            pnn.img = new Bitmap[10];
            
            for (int i = 0; i < 10; i++)
            {
                string ss = "e" + (i + 1) + ".png";
                pnn.img[i] = new Bitmap(ss);
                pnn.H = 96;
                pnn.W = 56;
            }
            pnn.Xdir = -1;
            pnn.frame = 1;
            pnn.X = LShapes[0].X + LShapes[0].W - pnn.W;
            pnn.Y = LShapes[0].Y-pnn.H;

            
            LEnemy.Add(pnn);


            //enemy in shape[3]
            pnn = new MActor();
            pnn.img = new Bitmap[10];
            pnn.X = LShapes[3].X;
            for (int i = 0; i < 10; i++)
            {
                string ss = "e" + (i + 1) + ".png";
                pnn.img[i] = new Bitmap(ss);
                pnn.W = 56;
                pnn.H = 96;
            }
            pnn.Xdir = 1;
            pnn.frame = 0;
            pnn.Y = LShapes[3].Y - pnn.H;
           
            LEnemy.Add(pnn);

            pnn = new MActor();
            pnn.img = new Bitmap[10];

            for (int i = 0; i < 10; i++)
            {
                string ss = "e" + (i + 1) + ".png";
                pnn.img[i] = new Bitmap(ss);
                pnn.H = 96;
                pnn.W = 56;
            }
            pnn.Xdir = -1;
            pnn.frame = 1;
            pnn.X = LShapes[3].X + LShapes[3].W - pnn.W;
            pnn.Y = LShapes[3].Y - pnn.H;

           
            LEnemy.Add(pnn);
            //enemy in shape[6]
            pnn = new MActor();
            pnn.img = new Bitmap[12];

            for (int i = 0; i < 12; i++)
            {
                string ss = "a" + (i + 1) + ".png";
                pnn.img[i] = new Bitmap(ss);
                pnn.H = 96;
                pnn.W = 56;
            }
            pnn.Xdir = 1;
            pnn.frame = 0;
            pnn.X = LShapes[6].X ;
            pnn.Y = LShapes[6].Y - pnn.H;


            LEnemy.Add(pnn);

            //enemy in shape[6]
            pnn = new MActor();
            pnn.img = new Bitmap[12];

            for (int i = 0; i < 12; i++)
            {
                string ss = "a" + (i + 1) + ".png";
                pnn.img[i] = new Bitmap(ss);
                pnn.H = 96;
                pnn.W = 56;
            }
            pnn.Xdir = -1;
            pnn.frame = 1;
            pnn.X = LShapes[6].X + LShapes[6].W-pnn.W;
            pnn.Y = LShapes[6].Y - pnn.H;


            LEnemy.Add(pnn);
        }


        void CreateSingleEnemy()
        {
            //enemy in shape[1]
            MActor pnn = new MActor();
            pnn.img = new Bitmap[10];
           
            for (int i = 0; i < 10; i++)
            {
                string ss = "e" + (i + 1) + ".png";
                pnn.img[i] = new Bitmap(ss);
                pnn.W = 56;
                pnn.H = 96;
            }
            pnn.X = LShapes[1].X + LShapes[1].W /2 ;
            pnn.Y = LShapes[1].Y - pnn.H;
            
            LSEnemy.Add(pnn);

            //enemy in shape[4]
             pnn = new MActor();
            pnn.img = new Bitmap[10];

            for (int i = 0; i < 10; i++)
            {
                string ss = "e" + (i + 1) + ".png";
                pnn.img[i] = new Bitmap(ss);
                pnn.W = 56;
                pnn.H = 96;
            }
            pnn.X = LShapes[4].X + LShapes[4].W / 2;
            pnn.Y = LShapes[4].Y - pnn.H;

           
            LSEnemy.Add(pnn);


            //enemy in shape[2]
            pnn = new MActor();
            pnn.img = new Bitmap[10];

            for (int i = 0; i < 10; i++)
            {
                string ss = "e" + (i + 1) + ".png";
                pnn.img[i] = new Bitmap(ss);
                pnn.W = 56;
                pnn.H = 96;
            }
            pnn.X = LShapes[2].X + LShapes[2].W / 2 +40;
            pnn.Y = LShapes[2].Y - pnn.H;

            
            LSEnemy.Add(pnn);
        }
        void CreateBullet(int x,int y,int xdir,int bulletHero)
        {
            if (xdir == 1)
            {
                SActor pnn = new SActor();
                pnn.img = new Bitmap("bullet.png");
                
                pnn.X = x+56;
                pnn.Y = y + 20;
                pnn.W = 20;
                pnn.H = 10;
                pnn.xdir = xdir;
                pnn.bulletHero = bulletHero;
                pnn.xold=pnn.X;

                LBullet.Add(pnn);
            }
            else
            {

                SActor pnn = new SActor();
                pnn.img = new Bitmap("bullet2.png");
                pnn.X = x;
                pnn.Y = y + 20;
                pnn.W = 20;
                pnn.H = 10;
                pnn.xdir = xdir;
                pnn.bulletHero = bulletHero;
                pnn.xold = pnn.X;
                LBullet.Add(pnn);
            }
        }

        void CreateLazer(int x, int y,int xdir,int bullethero)
        {
            Lazer = new List<SActor>();
            if (xdir == 1)
            {
                SActor pnn = new SActor();
                pnn.img = new Bitmap("ammobox3.png");

                pnn.X = x + 56;
                pnn.Y = y + 20;
                pnn.W = 200;
                pnn.H = 20;
                pnn.xdir = xdir;
                pnn.bulletHero = bullethero;
                Lazer.Add(pnn);
            }
            else
            {

                SActor pnn = new SActor();
                pnn.img = new Bitmap("ammobox3.png");
                pnn.X = x-200;
                pnn.Y = y + 20;
                pnn.W = 200;
                pnn.H = 20;
                pnn.xdir = xdir;
                pnn.bulletHero = bullethero;
                Lazer.Add(pnn);
            }
            lazer = true;
        }
        
        
        void CreateLudder()
        {
            SActor pnn = new SActor();
            pnn.img = new Bitmap("ludder.png");
            pnn.X = LShapes[0].X +20;
            pnn.Y= LShapes[0].Y;
            pnn.W = 100;
            pnn.H= this.Height  - 50;

            Ludder.Add(pnn);

            pnn = new SActor();
            pnn.img = new Bitmap("ludder.png");
            pnn.X = LShapes[2].X + LShapes[2].W / 2 -20;
            pnn.Y = LShapes[2].Y;
            pnn.W = 100;
            pnn.H = this.Height - 50;

            Ludder.Add(pnn);

            pnn = new SActor();
            pnn.img = new Bitmap("ludder.png");
            pnn.X = LShapes[6].X + 20;
            pnn.Y = LShapes[6].Y;
            pnn.W = 100;
            pnn.H = LShapes[3].Y- LShapes[6].Y;

            Ludder.Add(pnn);
        }
        void CreateHero()
        {
            MActor pnn = new MActor();
             pnn.img = new Bitmap[10];
            pnn.X = 20;
            for (int i = 0; i < 10; i++)
            {
                string ss = "h" + (i+1) +".png";
                pnn.img[i] = new Bitmap(ss);
                
                pnn.W = 56;
                pnn.H = 96;
                
            }


            pnn.Y = this.Height - pnn.H - 50;
            LHero.Add(pnn);


       


        }
        void ModifyRects()
        {
            LWorld[0].rcSrc = new Rectangle(XScroll, YScroll, ClientSize.Width, ClientSize.Height);
        }
        void CreateWorld()
        {
            CActor pnn = new CActor();
            pnn.img = new Bitmap("Rrrr.jpg");
            pnn.rcDst = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            pnn.rcSrc = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);

            LWorld.Add(pnn);



        }
        void CreateShapes()
        {
            //0
            SActor pnn = new SActor();
            pnn.img = new Bitmap("obs1.png");
            pnn.X = 0;
            pnn.Y = this.Height * 1 / 3;
            pnn.W = pnn.img.Width;
            pnn.H = pnn.img.Height;

            LShapes.Add(pnn);
            //1
            pnn = new SActor();
            pnn.img = new Bitmap("obs3.png");
            pnn.X = pnn.img.Width*3;
            pnn.Y = this.Height * 2 / 3;
            pnn.W = pnn.img.Width;
            pnn.H = pnn.img.Height;

            LShapes.Add(pnn);

            //2
            pnn = new SActor();
            pnn.img = new Bitmap("obs3.png");
            pnn.X = pnn.img.Width * 4;
            pnn.Y = this.Height * 1 / 4;
            pnn.W = pnn.img.Width;
            pnn.H = pnn.img.Height;

            LShapes.Add(pnn);

           
            //3
            pnn = new SActor();
            pnn.img = new Bitmap("obs1.png");
            pnn.X = pnn.img.Width * 3;
            pnn.Y = this.Height * 2 / 4;
            pnn.W = pnn.img.Width;
            pnn.H = pnn.img.Height;

            LShapes.Add(pnn);
            //4
            pnn = new SActor();
            pnn.img = new Bitmap("obs3.png");
            pnn.X = pnn.img.Width * 5;
            pnn.Y = this.Height * 2 / 4;
            pnn.W = pnn.img.Width;
            pnn.H = pnn.img.Height;

            LShapes.Add(pnn);
            //5
            pnn = new SActor();
            pnn.img = new Bitmap("obs3.png");
            pnn.X = pnn.img.Width * 6;
            pnn.Y = this.Height -pnn.img.Height-50;
            pnn.W = pnn.img.Width;
            pnn.H = pnn.img.Height;

            LShapes.Add(pnn);


            //6
            pnn = new SActor();
            pnn.img = new Bitmap("obs1.png");
            pnn.X = pnn.img.Width * 3 + 400;
            pnn.Y = this.Height * 1 / 6;
            pnn.W = pnn.img.Width;
            pnn.H = pnn.img.Height;

            LShapes.Add(pnn);

            //key

            pnn = new SActor();
            pnn.img = new Bitmap("key.png");
            pnn.X = LShapes[6].X + LShapes[6].W-30;
            pnn.Y = LShapes[6].Y -50;
            pnn.W = 30;
            pnn.H = 50;

            Litems.Add(pnn);
            // car

            pnn = new SActor();
            pnn.img = new Bitmap("car.png");
            pnn.X = LShapes[6].X + 400 ;
            pnn.Y = this.Height - 200 - 50;
            pnn.W = 200 ;
            pnn.H = 200;

            Litems.Add(pnn);
            
        }
        void DrawScene(Graphics g)
        {
            g.Clear(Color.Black);
            if (ShowStart == true  || ShowEnd == true  || ShowWinner == true)
            {
                for (int i = 0; i < LMenu.Count; i++)
                {
                    if (ShowStart == true && i == 0 || ShowEnd == true && i == 1 || ShowWinner == true && i == 2)
                    {
                        g.DrawImage(LMenu[i].img, LMenu[i].X, LMenu[i].Y, LMenu[i].W, LMenu[i].H);
                    }

                }
            }
            else
            {
                for (int i = 0; i < LWorld.Count; i++)
                {
                    g.DrawImage(LWorld[i].img, LWorld[i].rcDst, LWorld[i].rcSrc, GraphicsUnit.Pixel);
                }

                for (int i = 0; i < LShapes.Count; i++)
                {
                    g.DrawImage(LShapes[i].img, LShapes[i].X, LShapes[i].Y, LShapes[i].W, LShapes[i].H);
                }
                for (int i = 0; i < LEnemy.Count; i++)
                {
                    g.DrawImage(LEnemy[i].img[LEnemy[i].frame], LEnemy[i].X, LEnemy[i].Y, LEnemy[i].W, LEnemy[i].H);

                }

                for (int i = 0; i < LSEnemy.Count; i++)
                {
                    g.DrawImage(LSEnemy[i].img[LSEnemy[i].frame], LSEnemy[i].X, LSEnemy[i].Y, LSEnemy[i].W, LSEnemy[i].H);

                }

                for (int i = 0; i < Ludder.Count; i++)
                {
                    g.DrawImage(Ludder[i].img, Ludder[i].X, Ludder[i].Y, Ludder[i].W, Ludder[i].H);
                }

                for (int i = 0; i < LBullet.Count; i++)
                {
                    g.DrawImage(LBullet[i].img, LBullet[i].X, LBullet[i].Y, LBullet[i].W, LBullet[i].H);
                }

                for (int i = 0; i < Lazer.Count; i++)
                {
                    g.DrawImage(Lazer[i].img, Lazer[i].X, Lazer[i].Y, Lazer[i].W, Lazer[i].H);
                }
                for (int i = 0; i < LHbar.Count; i++)
                {
                    if (HeroRideCar == false)
                    {
                        g.DrawImage(LHbar[i].img[LHbar[i].frame], LHbar[i].X, LHbar[i].Y, LHbar[i].W, LHbar[i].H);
                    }

                }

                for (int i = 0; i < LDHbar.Count; i++)
                {
                    g.DrawImage(LDHbar[i].img[LDHbar[i].frame], LDHbar[i].X, LDHbar[i].Y, LDHbar[i].W, LDHbar[i].H);

                }

                for (int i = 0; i < LSHbar.Count; i++)
                {
                    g.DrawImage(LSHbar[i].img[LSHbar[i].frame], LSHbar[i].X, LSHbar[i].Y, LSHbar[i].W, LSHbar[i].H);

                }

                for (int i = 0; i < LAmmoBox.Count; i++)
                {
                    g.DrawImage(LAmmoBox[i].img[LAmmoBox[i].frame], LAmmoBox[i].X, LAmmoBox[i].Y, LAmmoBox[i].W, LAmmoBox[i].H);

                }

                for (int i = 0; i < Litems.Count; i++)
                {
                    if (drawKey == false && i == 0)
                    {
                        i++;
                    }
                    g.DrawImage(Litems[i].img, Litems[i].X, Litems[i].Y, Litems[i].W, Litems[i].H);
                    //this.Text = Litems.Count + "";
                }

               

                if (HeroRideCar == false)
                {
                    g.DrawImage(LHero[0].img[LHero[0].frame], LHero[0].X, LHero[0].Y, LHero[0].W, LHero[0].H);
                   
                }
            }
            

        }

        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);

        }
    }
}
