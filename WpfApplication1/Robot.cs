using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Robot
    {
        SerialPort2Dynamixel serialPort = new SerialPort2Dynamixel();
        Dynamixel dynamixel = new Dynamixel();
        String com; //Puerto COM

        public Robot(String com)
        {
            this.com = com;
        }

        public void atacar()
        {
            if (serialPort.open(com) == false)
            {
                dynamixel.sendTossModeCommand(serialPort);

                dynamixel.setPosition(serialPort, 1, 512);
                dynamixel.setPosition(serialPort, 2, 511);
                dynamixel.setPosition(serialPort, 3, 412);
                dynamixel.setPosition(serialPort, 4, 612);
                dynamixel.setPosition(serialPort, 5, 372);
                dynamixel.setPosition(serialPort, 6, 651);
                dynamixel.setPosition(serialPort, 7, 512);
                dynamixel.setPosition(serialPort, 8, 512);
                dynamixel.setPosition(serialPort, 9, 442);
                dynamixel.setPosition(serialPort, 10, 581);
                dynamixel.setPosition(serialPort, 11, 612);
                dynamixel.setPosition(serialPort, 12, 412);
                dynamixel.setPosition(serialPort, 13, 512);
                dynamixel.setPosition(serialPort, 14, 512);
                dynamixel.setPosition(serialPort, 11, 642);
                dynamixel.setPosition(serialPort, 12, 649);
                dynamixel.setPosition(serialPort, 13, 634);
                dynamixel.setPosition(serialPort, 14, 607);

                Thread.Sleep(24);

                //Paso1
                dynamixel.setPosition(serialPort, 2, 512);
                dynamixel.setPosition(serialPort, 5, 312);
                dynamixel.setPosition(serialPort, 6, 711);
                dynamixel.setPosition(serialPort, 9, 392);
                dynamixel.setPosition(serialPort, 10, 622);
                dynamixel.setPosition(serialPort, 11, 572);
                dynamixel.setPosition(serialPort, 12, 452);
                dynamixel.setPosition(serialPort, 13, 532);
                dynamixel.setPosition(serialPort, 14, 492);
                dynamixel.setPosition(serialPort, 16, 871);
                dynamixel.setPosition(serialPort, 17, 797);
                dynamixel.setPosition(serialPort, 18, 567);

                Thread.Sleep(464);

                //Paso2
                dynamixel.setPosition(serialPort, 2, 511);
                dynamixel.setPosition(serialPort, 5, 372);
                dynamixel.setPosition(serialPort, 6, 651);
                dynamixel.setPosition(serialPort, 9, 442);
                dynamixel.setPosition(serialPort, 10, 581);
                dynamixel.setPosition(serialPort, 11, 612);
                dynamixel.setPosition(serialPort, 12, 412);
                dynamixel.setPosition(serialPort, 13, 512);
                dynamixel.setPosition(serialPort, 14, 512);
                dynamixel.setPosition(serialPort, 16, 654);
                dynamixel.setPosition(serialPort, 17, 722);
                dynamixel.setPosition(serialPort, 18, 740);

                serialPort.close();
            }
            else
            {
                Console.Out.WriteLine("\nNo puedo hablar con el robot"); //No se puede abrir el puerto serial
            }
        }

        public void caminar()
        {
            if (serialPort.open(com) == false)
            {
                dynamixel.sendTossModeCommand(serialPort);

                for(int i=0; i<4; i++)
                {
                    //Play 18
                    //Paso0
                    dynamixel.setPosition(serialPort, 3, 501);
                    dynamixel.setPosition(serialPort, 4, 672);
                    dynamixel.setPosition(serialPort, 5, 512);
                    dynamixel.setPosition(serialPort, 6, 581);
                    dynamixel.setPosition(serialPort, 7, 511);
                    dynamixel.setPosition(serialPort, 8, 511);
                    dynamixel.setPosition(serialPort, 9, 441);
                    dynamixel.setPosition(serialPort, 10, 541);
                    dynamixel.setPosition(serialPort, 11, 641);
                    dynamixel.setPosition(serialPort, 12, 501);
                    dynamixel.setPosition(serialPort, 13, 511);
                    dynamixel.setPosition(serialPort, 14, 581);

                    //Paso1
                    dynamixel.setPosition(serialPort, 5, 442);
                    dynamixel.setPosition(serialPort, 6, 511);
                    dynamixel.setPosition(serialPort, 9, 481);
                    dynamixel.setPosition(serialPort, 10, 601);
                    dynamixel.setPosition(serialPort, 13, 441);
                    dynamixel.setPosition(serialPort, 14, 511);

                    //Paso2
                    dynamixel.setPosition(serialPort, 3, 352);
                    dynamixel.setPosition(serialPort, 4, 522);
                    dynamixel.setPosition(serialPort, 7, 512);
                    dynamixel.setPosition(serialPort, 8, 512);
                    dynamixel.setPosition(serialPort, 9, 482);
                    dynamixel.setPosition(serialPort, 10, 582);
                    dynamixel.setPosition(serialPort, 11, 522);
                    dynamixel.setPosition(serialPort, 12, 382);
                    dynamixel.setPosition(serialPort, 13, 442);
                    dynamixel.setPosition(serialPort, 14, 512);
                    //Paso3

                    dynamixel.setPosition(serialPort, 5, 512);
                    dynamixel.setPosition(serialPort, 6, 581);
                    dynamixel.setPosition(serialPort, 9, 422);
                    dynamixel.setPosition(serialPort, 10, 542);
                    dynamixel.setPosition(serialPort, 13, 512);
                    dynamixel.setPosition(serialPort, 14, 582);
                }
                serialPort.close();
            }
            else
            {
                Console.Out.WriteLine("\nNo puedo hablar con el robot"); //No se puede abrir el puerto serial
            }
        }

        public void derecha()
        {
            if (serialPort.open(com) == false)
            {
                dynamixel.sendTossModeCommand(serialPort);

                for (int i=0; i<3; i++)
                {
                    //Play 18
                    //Paso0
                    dynamixel.setPosition(serialPort, 3, 412);
                    dynamixel.setPosition(serialPort, 4, 512);
                    dynamixel.setPosition(serialPort, 5, 442);
                    dynamixel.setPosition(serialPort, 6, 512);
                    dynamixel.setPosition(serialPort, 7, 512);
                    dynamixel.setPosition(serialPort, 8, 512);
                    dynamixel.setPosition(serialPort, 9, 512);
                    dynamixel.setPosition(serialPort, 10, 512);
                    dynamixel.setPosition(serialPort, 11, 612);
                    dynamixel.setPosition(serialPort, 12, 412);
                    dynamixel.setPosition(serialPort, 13, 442);
                    dynamixel.setPosition(serialPort, 14, 512);

                    //Paso1
                    dynamixel.setPosition(serialPort, 3, 361);
                    dynamixel.setPosition(serialPort, 4, 511);
                    dynamixel.setPosition(serialPort, 5, 441);
                    dynamixel.setPosition(serialPort, 6, 511);
                    dynamixel.setPosition(serialPort, 7, 511);
                    dynamixel.setPosition(serialPort, 8, 511);
                    dynamixel.setPosition(serialPort, 9, 511);
                    dynamixel.setPosition(serialPort, 10, 511);
                    dynamixel.setPosition(serialPort, 11, 511);
                    dynamixel.setPosition(serialPort, 12, 411);
                    dynamixel.setPosition(serialPort, 13, 441);
                    dynamixel.setPosition(serialPort, 14, 511);

                    //Paso2
                    dynamixel.setPosition(serialPort, 3, 362);
                    dynamixel.setPosition(serialPort, 4, 512);
                    dynamixel.setPosition(serialPort, 5, 512);
                    dynamixel.setPosition(serialPort, 6, 512);
                    dynamixel.setPosition(serialPort, 7, 512);
                    dynamixel.setPosition(serialPort, 8, 512);
                    dynamixel.setPosition(serialPort, 9, 512);
                    dynamixel.setPosition(serialPort, 10, 512);
                    dynamixel.setPosition(serialPort, 11, 512);
                    dynamixel.setPosition(serialPort, 12, 412);
                    dynamixel.setPosition(serialPort, 13, 512);
                    dynamixel.setPosition(serialPort, 14, 512);
                    //Paso3

                    dynamixel.setPosition(serialPort, 9, 442);
                    //Paso4
                    dynamixel.setPosition(serialPort, 3, 512);
                    dynamixel.setPosition(serialPort, 11, 612);

                    //Play 19
                    //Paso0
                    dynamixel.setPosition(serialPort, 3, 512);
                    dynamixel.setPosition(serialPort, 4, 512);
                    dynamixel.setPosition(serialPort, 5, 512);
                    dynamixel.setPosition(serialPort, 6, 512);
                    dynamixel.setPosition(serialPort, 7, 512);
                    dynamixel.setPosition(serialPort, 8, 512);
                    dynamixel.setPosition(serialPort, 9, 512);
                    dynamixel.setPosition(serialPort, 10, 582);
                    dynamixel.setPosition(serialPort, 11, 612);
                    dynamixel.setPosition(serialPort, 12, 412);
                    dynamixel.setPosition(serialPort, 13, 512);
                    dynamixel.setPosition(serialPort, 14, 512);

                    //Paso1
                    dynamixel.setPosition(serialPort, 4, 662);
                    dynamixel.setPosition(serialPort, 12, 512);

                    //Paso2
                    dynamixel.setPosition(serialPort, 10, 512);

                    //Paso3                                
                    dynamixel.setPosition(serialPort, 6, 582);
                    dynamixel.setPosition(serialPort, 13, 512);
                    dynamixel.setPosition(serialPort, 14, 582);

                    //Paso4
                    dynamixel.setPosition(serialPort, 4, 612);
                    dynamixel.setPosition(serialPort, 12, 412);
                }

                serialPort.close();
            }
            else
            {
                Console.Out.WriteLine("\nNo puedo hablar con el robot"); //No se puede abrir el puerto serial
            }
        }

        public void izquierda()
        {
            if (serialPort.open(com) == false)
            {
                dynamixel.sendTossModeCommand(serialPort);
                
                for(int i=0;i<4;i++)
                {
                    //Play 18
                    //Paso0
                    dynamixel.setPosition(serialPort, 3, 512);
                    dynamixel.setPosition(serialPort, 4, 612);
                    dynamixel.setPosition(serialPort, 5, 512);
                    dynamixel.setPosition(serialPort, 6, 582);
                    dynamixel.setPosition(serialPort, 7, 512);
                    dynamixel.setPosition(serialPort, 8, 512);
                    dynamixel.setPosition(serialPort, 9, 512);
                    dynamixel.setPosition(serialPort, 10, 512);
                    dynamixel.setPosition(serialPort, 11, 612);
                    dynamixel.setPosition(serialPort, 12, 412);
                    dynamixel.setPosition(serialPort, 13, 512);
                    dynamixel.setPosition(serialPort, 14, 582);

                    //Paso1
                    dynamixel.setPosition(serialPort, 4, 662);
                    dynamixel.setPosition(serialPort, 12, 512);


                    //Paso2
                    dynamixel.setPosition(serialPort, 6, 512);
                    dynamixel.setPosition(serialPort, 14, 512);

                    //Paso3

                    dynamixel.setPosition(serialPort, 10, 582);
                    //Paso4
                    dynamixel.setPosition(serialPort, 4, 512);
                    dynamixel.setPosition(serialPort, 12, 412);

                    //Play 19
                    //Paso0
                    dynamixel.setPosition(serialPort, 3, 512);
                    dynamixel.setPosition(serialPort, 4, 512);
                    dynamixel.setPosition(serialPort, 5, 512);
                    dynamixel.setPosition(serialPort, 6, 512);
                    dynamixel.setPosition(serialPort, 7, 512);
                    dynamixel.setPosition(serialPort, 8, 512);
                    dynamixel.setPosition(serialPort, 9, 442);
                    dynamixel.setPosition(serialPort, 10, 512);
                    dynamixel.setPosition(serialPort, 11, 612);
                    dynamixel.setPosition(serialPort, 12, 412);
                    dynamixel.setPosition(serialPort, 13, 512);
                    dynamixel.setPosition(serialPort, 14, 512);

                    //Paso1
                    dynamixel.setPosition(serialPort, 3, 362);
                    dynamixel.setPosition(serialPort, 11, 512);
                    
                    //Paso2
                    dynamixel.setPosition(serialPort, 9, 512);

                    //Paso3                                
                    dynamixel.setPosition(serialPort, 5, 442);
                    dynamixel.setPosition(serialPort, 13, 442);

                    //Paso4
                    dynamixel.setPosition(serialPort, 3, 412);
                    dynamixel.setPosition(serialPort, 11, 612);
                }
                
                serialPort.close();

            }
            else
            {
                Console.Out.WriteLine("\nNo puedo hablar con el robot"); //No se puede abrir el puerto serial
            }
        }
    }
}
