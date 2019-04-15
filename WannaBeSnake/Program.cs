using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;


namespace WannaBeSnake
{
    class Program
    {
        public struct Position
        {
            public int X, Y;
            public Position(int x ,int y)
            {
                X = x;
                Y = y;
            }
        }

        public struct Manzanita
        {
            public int X, Y;
            public Manzanita(int x, int y)
            {
                X = x;
                Y = y;
            }

            public void Respawn()
            {
                Random rnd = new Random();
                X = rnd.Next(2, 148);
                Y = rnd.Next(0, 28);
            }

            public void Draw()
            {
                Console.SetCursorPosition(X, Y);
                Console.BackgroundColor=ConsoleColor.Black;
                Console.Write("@");
            }
        }

        public struct Snake
        {
            public int X, Y, dX, dY, bodyLen, steps, appleX, appleY;
            public List<Position> bodyPositions;
            public List<Position> smartPath;
            public bool bodyFull, grow, isSmart, newGoal;

            public Snake(int x,int y)
            {
                X = x;
                Y = y;
                dX = 1;
                dY = 0;                
                bodyLen = 1;
                bodyFull = false;
                grow = false;
                bodyPositions = new List<Position>();
                smartPath = new List<Position>();                
                appleX = 0;
                appleY = 0;
                newGoal = true;
                isSmart = false;
                steps = 0;
            }

            public void loseMessage()
            {
                //MessageBox.Show("PERDISTE CON UN PUNTAJE TOTAL DE -> " + (bodyLen - 1), "Aceptar");
                //reborn();
            }

            public void reborn()
            {
                X = 10;
                Y = 10;
                dX = 1;
                dY = 0;
                bodyLen = 1;
                bodyFull = false;
                grow = false;
                bodyPositions = new List<Position>();
                Console.Clear();
            }
            
            public void redirect(ConsoleKey myKey)
            {
                
                switch (myKey)
                {
                    case ConsoleKey.W:
                        if(dY != 1)
                        {
                            dY = -1;
                            dX = 0;
                        }                        
                        break;
                    case ConsoleKey.S:
                        if(dY != -1)
                        {
                            dY = 1;
                            dX = 0;
                        }                        
                        break;
                    case ConsoleKey.A:
                        if(dX != 1)
                        {
                            dY = 0;
                            dX = -1;
                        }
                        break;
                    case ConsoleKey.D:
                        if(dX != -1)
                        {
                            dY = 0;
                            dX = 1;
                        }
                        break;
                }         
                
            }

            public int distance(int x1, int y1, int x2, int y2)
            {
                return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
            }

            public bool autoKill(Position p)
            {
                for(int i = 0; i < bodyPositions.Count; i++)
                {
                    if(p.X == bodyPositions[i].X && p.Y == bodyPositions[i].Y)
                    {
                        return true;
                    }
                }

                return false;
            }

            //Hill Climbing
            public void smartMove()
            {
                
                if (newGoal)
                {
                    Position goal = new Position(appleX, appleY);
                    //Comienza 
                    Position sucesor = new Position(X, Y);
                    bool success = false;
                    int tempX = X, tempY = Y;
                    smartPath = new List<Position>();
                    steps = 0;

                    while (!success)
                    {
                        

                        //Si llega a la meta lo agregamos al camino
                        if ((sucesor.X + 1) == goal.X && sucesor.Y == goal.Y)
                        {
                            sucesor.X += 1;
                            smartPath.Add(sucesor);
                            success = true;
                            break;
                        }
                        else if ((sucesor.X) == goal.X && (sucesor.Y + 1) == goal.Y)
                        {
                            sucesor.Y += 1;
                            smartPath.Add(sucesor);
                            success = true;
                            break;
                        }
                        else if ((sucesor.X - 1) == goal.X && sucesor.Y == goal.Y)
                        {
                            sucesor.X -= 1;
                            smartPath.Add(sucesor);
                            success = true;
                            break;
                        }
                        else if ((sucesor.X) == goal.X && (sucesor.Y - 1) == goal.Y)
                        {
                            sucesor.Y -= 1;
                            smartPath.Add(sucesor);
                            success = true;
                            break;
                        }
                        
                            if( distance(sucesor.X+1,sucesor.Y,goal.X,goal.Y) < distance(sucesor.X - 1, sucesor.Y, goal.X, goal.Y))
                            {
                                if(!autoKill(new Position(sucesor.X + 1, sucesor.Y)))
                                {
                                    tempX = sucesor.X + 1;
                                    tempY = sucesor.Y;
                                }
                            tempX = sucesor.X + 1;
                            tempY = sucesor.Y;
                        }
                            else
                            {
                                if (!autoKill(new Position(sucesor.X - 1, sucesor.Y)))
                                {
                                    tempX = sucesor.X - 1;
                                    tempY = sucesor.Y;
                                }
                            tempX = sucesor.X - 1;
                            tempY = sucesor.Y;

                        }

                            if(distance(sucesor.X, sucesor.Y - 1, goal.X, goal.Y) < distance(tempX, tempY, goal.X, goal.Y))
                            {
                                if (!autoKill(new Position(sucesor.X, sucesor.Y - 1)))
                                {
                                    tempX = sucesor.X;
                                    tempY = sucesor.Y - 1;
                                }
                            tempX = sucesor.X;
                            tempY = sucesor.Y - 1;

                        }

                            if (distance(sucesor.X, sucesor.Y + 1, goal.X, goal.Y) < distance(tempX, tempY, goal.X, goal.Y))
                            {
                                if (!autoKill(new Position(sucesor.X, sucesor.Y + 1)))
                                {
                                    tempX = sucesor.X;
                                    tempY = sucesor.Y + 1;
                                }

                            tempX = sucesor.X;
                            tempY = sucesor.Y + 1;
                        }

                            sucesor.X = tempX;
                            sucesor.Y = tempY;

                            smartPath.Add(sucesor);

                        
                    }

                    newGoal = false;
                }
            }
           

            public void moveSnake()
            {
                if (isSmart)
                {
                    /*
                    if (smartPath[steps].X > X)
                    {
                        dX = 1;
                        dY = 0;
                    }
                    if (smartPath[steps].X < X)
                    {
                        dX = -1;
                        dY = 0;
                    }
                    if (smartPath[steps].Y > Y)
                    {
                        dY = 1;
                        dX = 0;
                    }
                    if (smartPath[steps].Y < Y)
                    {
                        dY = -1;
                        dX = 0;
                    }*/

                    X = smartPath[steps].X;
                    Y = smartPath[steps].Y;

                    steps++;
                }
                else
                {
                    X += dX;
                    Y += dY;
                }

                          

                for(int i = 0; i<bodyPositions.Count; i++)
                {
                    if(X == bodyPositions[i].X && Y == bodyPositions[i].Y)
                    {
                        loseMessage();
                        break;
                    }
                }
                if (X < 0) X = 149;                
                if (Y < 0) Y = 29;
                if (X > 149) X = 0;
                if (Y > 29) Y = 0;

                drawSnake();
            }

            public void growUp()
            {
                bodyLen++;
                grow = true;
            }

            public void drawSnake()
            {
                if (!bodyFull)
                {
                    for(int i = 0; i<bodyLen; i++)
                    {
                        Console.SetCursorPosition(X, Y);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("*");
                        bodyPositions.Add(new Position(X, Y));
                        X++;
                    }
                    bodyFull = true;
                }
                else
                {
                    Console.SetCursorPosition(bodyPositions[0].X, bodyPositions[0].Y);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                    bodyPositions.RemoveAt(0);
                    Console.SetCursorPosition(X, Y);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("*");
                    bodyPositions.Add(new Position(X, Y));
                    if (grow)
                    {
                        X += dX;
                        Y += dY;
                        if (X < 1) X = 149;
                        if (Y < 1) Y = 29;
                        if (X > 149) X = 0;
                        if (Y > 29) Y = 0;
                        Console.SetCursorPosition(X, Y);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("*");
                        bodyPositions.Add(new Position(X, Y));
                        grow = false;
                    }
                }

            }
        }


    

        static void Main(string[] args)
        {
            
            Random rnd = new Random();
            Manzanita myApple = new Manzanita(20, 20);
            Console.CursorVisible = false;
            Console.SetWindowSize(150, 30);
            Snake mySnake = new Snake(10, 10);
            mySnake.isSmart = true;
            mySnake.appleX = myApple.X;
            mySnake.appleY = myApple.Y;
            var keyPressEvent = Task.Run(() => mySnake.redirect(Console.ReadKey(true).Key));
            ConsoleKey mykey = new ConsoleKey();

            var keyHold = Task.Run(() => mykey = Console.ReadKey(true).Key);
            while (true)
            {
                Console.SetCursorPosition(1, 0);
                Console.Write("MANZANITAS COMIDAS -> " + (mySnake.bodyLen - 1) + " - " + myApple.X + " - " + myApple.Y);
                Thread.Sleep(1);
                mySnake.smartMove();
                
                mySnake.moveSnake();
                myApple.Draw();
                if (mySnake.X == myApple.X && mySnake.Y == myApple.Y)
                {
                    Console.Beep(200, 200);
                    mySnake.growUp();
                    myApple.Respawn();
              
                    mySnake.newGoal = true;
                    mySnake.steps = 0;
                    myApple.Respawn();
                    mySnake.appleX = myApple.X;
                    mySnake.appleY = myApple.Y;
                }
                if (keyPressEvent.IsCompleted) keyPressEvent = Task.Run(() => mySnake.redirect(Console.ReadKey(true).Key));
                
            }            
        }

        public void ColorConsole(ConsoleColor color)
        {
            for (int i = 0; i < 150; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.BackgroundColor = color;
                }
            }
            Console.SetCursorPosition(0, 0);
        }

    }

 }



