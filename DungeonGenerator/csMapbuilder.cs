using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Engine;

using Rectangle = Engine.IntRect;
using Point = Engine.IntVector2;
using Size = Engine.IntSize2;

namespace RogueLikeMapBuilder
{
    /// <summary>
    /// This class demonstrates a simple map builder for a roguelike game. For a detailed
    /// look at using it, go here http://www.evilscience.co.uk/?p=553
    /// </summary>
    public class csMapbuilder
    {
        public UInt16[,] map;

        /// <summary>
        /// Built rooms stored here
        /// </summary>
        private List<Rectangle> rctBuiltRooms;

        /// <summary>
        /// Built corridors stored here
        /// </summary>
        private List<Point> lBuilltCorridors;

        /// <summary>
        /// Corridor to be built stored here
        /// </summary>
        private List<Point> lPotentialCorridor;

        private Dictionary<UInt16, UInt16> corridorTerminatingRooms = new Dictionary<UInt16, UInt16>();

        /// <summary>
        /// Room to be built stored here
        /// </summary>
        private Rectangle rctCurrentRoom;

        private Rectangle startRoom;
        private Rectangle endRoom;

        public Point? EastConnector { get; set; }

        public Point? WestConnector { get; set; }

        public Point? NorthConnector { get; set; }

        public Point? SouthConnector { get; set; }

        public Rectangle StartRoom => startRoom;

        public Rectangle EndRoom => endRoom;

        public List<Rectangle> Rooms => rctBuiltRooms;

        public List<Point> Corridors => lBuilltCorridors;

        public UInt16 WestConnectorIndex { get; private set; } = NullCell;

        public UInt16 EastConnectorIndex { get; private set; } = NullCell;

        public UInt16 NorthConnectorIndex { get; private set; } = NullCell;

        public UInt16 SouthConnectorIndex { get; private set; } = NullCell;


        #region builder public properties

        //room properties
        [Category("Room"), Description("Minimum Size"), DisplayName("Minimum Size")]
        public Size Room_Min { get; set; }
        [Category("Room"), Description("Max Size"), DisplayName("Maximum Size")]
        public Size Room_Max { get; set; }
        [Category("Room"), Description("Total number"), DisplayName("Rooms to build")]
        public int MaxRooms { get; set; }
        [Category("Room"), Description("Minimum distance between rooms"), DisplayName("Distance from other rooms")]
        public int RoomDistance { get; set; }
        [Category("Room"), Description("Minimum distance of room from existing corridors"), DisplayName("Corridor Distance")]
        public int CorridorDistance { get; set; }

        //corridor properties
        [Category("Corridor"), Description("Minimum corridor length"), DisplayName("Minimum length")]
        public int Corridor_Min { get; set; }
        [Category("Corridor"), Description("Maximum corridor length"), DisplayName("Maximum length")]
        public int Corridor_Max { get; set; }
        [Category("Corridor"), Description("Maximum turns"), DisplayName("Maximum turns")]
        public int Corridor_MaxTurns { get; set; }
        [Category("Corridor"), Description("The distance a corridor has to be away from a closed cell for it to be built"), DisplayName("Corridor Spacing")]
        public int CorridorSpace { get; set; }


        [Category("Probability"), Description("Probability of building a corridor from a room or corridor. Greater than value = room"), DisplayName("Select room")]
        public int BuildProb { get; set; }

        [Category("Map"), DisplayName("Map Size")]
        public Size Map_Size { get; set; }
        [Category("Map"), DisplayName("Break Out"), Description("")]
        public int BreakOut { get; set; }

        /// <summary>
        /// Allow corrdiors that connect to existing rooms / corridors. If this is false 
        /// any of these created will not be added into the final map. 
        /// Default: false, but the original algo would be true.
        /// </summary>
        public bool AllowOtherCorridors { get; set; }

        /// <summary>
        /// Set this to true to give the room a horizontal, east/west layout. False to be vertical, north/south.
        /// </summary>
        public bool Horizontal { get; set; }

        #endregion

        /// <summary>
        /// describes the outcome of the corridor building operation
        /// </summary>
        enum CorridorItemHit
        {

            invalid //invalid point generated
            ,
            self  //corridor hit self
                ,
            existingcorridor //hit a built corridor
                ,
            originroom //corridor hit origin room 
                ,
            existingroom //corridor hit existing room
                ,
            completed //corridor built without problem    
                ,
            tooclose
                , OK //point OK
        }

        Point[] directions_straight = new Point[]{
                                            new Point(0, -1) //n
                                            , new Point(0, 1)//s
                                            , new Point(1, 0)//w
                                            , new Point(-1, 0)//e
                                        };

        public const UInt16 EmptyCell = 0;
        public const UInt16 RoomCell = 1;
        public const UInt16 CorridorCell = UInt16.MaxValue / 3;
        public const UInt16 OtherCorridorCell = (UInt16)((int)UInt16.MaxValue * 2 / 3);
        public const UInt16 MainCorridorCell = CorridorCell;
        public const UInt16 NullCell = UInt16.MaxValue;

        private UInt16 currentRoomCell = RoomCell;
        private UInt16 currentCorridorCell = CorridorCell;
        private UInt16 currentOtherCorridorCell = OtherCorridorCell;
        private Random rnd;

        public csMapbuilder(Random random, int x, int y)
        {
            this.rnd = random;
            Map_Size = new Size(x, y);
            Corridor_MaxTurns = 5;
            Room_Min = new Size(3, 3);
            Room_Max = new Size(15, 15);
            Corridor_Min = 3;
            Corridor_Max = 15;
            MaxRooms = 15;

            RoomDistance = 5;
            CorridorDistance = 2;
            CorridorSpace = 2;

            BuildProb = 50;
            BreakOut = 250;
        }

        /// <summary>
        /// Initialise everything
        /// </summary>
        private void Clear()
        {
            lPotentialCorridor = new List<Point>();
            rctBuiltRooms = new List<Rectangle>();
            lBuilltCorridors = new List<Point>();
            currentRoomCell = RoomCell;
            currentCorridorCell = CorridorCell;
            currentOtherCorridorCell = OtherCorridorCell;
            corridorTerminatingRooms[csMapbuilder.CorridorCell] = csMapbuilder.RoomCell + 1; //This will always be true, first corridor connected to 2nd room

            map = new UInt16[Map_Size.Width, Map_Size.Height];
            for (int x = 0; x < Map_Size.Width; x++)
                for (int y = 0; y < Map_Size.Height; y++)
                    map[x, y] = EmptyCell;
        }

        #region build methods()

        /// <summary>
        /// Randomly choose a room and attempt to build a corridor terminated by
        /// a room off it, and repeat until MaxRooms has been reached. The map
        /// is started of by placing a room in approximately the centre of the map
        /// using the method PlaceStartRoom()
        /// </summary>
        /// <returns>Bool indicating if the map was built, i.e. the property BreakOut was not
        /// exceed</returns>
        public bool Build_OneStartRoom()
        {
            int loopctr = 0;

            CorridorItemHit CorBuildOutcome;
            Point Location = new Point();
            Point Direction = new Point();

            Clear();

            PlaceStartRoom();

            //attempt to build the required number of rooms
            while (rctBuiltRooms.Count() < MaxRooms)
            {

                if (loopctr++ > BreakOut)//bail out if this value is exceeded
                    return false;

                if (Corridor_GetStart(out Location, out Direction))
                {

                    CorBuildOutcome = CorridorMake_Straight(ref Location, ref Direction, rnd.Next(1, Corridor_MaxTurns)
                        , rnd.Next(0, 100) > 50 ? true : false);

                    switch (CorBuildOutcome)
                    {
                        case CorridorItemHit.existingroom:
                        case CorridorItemHit.existingcorridor:
                        case CorridorItemHit.self:
                            Corridor_Build(false);
                            break;

                        case CorridorItemHit.completed:
                            if (Room_AttemptBuildOnCorridor(Direction))
                            {
                                RecordCurrentCorridorTerminatingRoom();
                                Corridor_Build(true);
                                Room_Build();
                            }
                            break;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Randomly choose a room and attempt to build a corridor terminated by
        /// a room off it, and repeat until MaxRooms has been reached. The map
        /// is started of by placing two rooms on opposite sides of the map and joins
        /// them with a long corridor, using the method PlaceStartRooms()
        /// </summary>
        /// <returns>Bool indicating if the map was built, i.e. the property BreakOut was not
        /// exceed</returns>
        public bool Build_ConnectedStartRooms()
        {
            int loopctr = 0;

            CorridorItemHit CorBuildOutcome;
            Point Location = new Point();
            Point Direction = new Point();

            Clear();

            PlaceStartRooms();

            //attempt to build the required number of rooms
            while (rctBuiltRooms.Count() < MaxRooms)
            {

                if (loopctr++ > BreakOut)//bail out if this value is exceeded
                    return false;

                if (Corridor_GetStart(out Location, out Direction))
                {

                    CorBuildOutcome = CorridorMake_Straight(ref Location, ref Direction, rnd.Next(1, Corridor_MaxTurns)
                        , rnd.Next(0, 100) > 50 ? true : false);

                    switch (CorBuildOutcome)
                    {
                        case CorridorItemHit.existingroom:
                        case CorridorItemHit.existingcorridor:
                        case CorridorItemHit.self:
                            Corridor_Build(false);
                            break;

                        case CorridorItemHit.completed:
                            if (Room_AttemptBuildOnCorridor(Direction))
                            {
                                RecordCurrentCorridorTerminatingRoom();
                                Corridor_Build(true);
                                Room_Build();
                            }
                            break;
                    }
                }
            }

            return true;

        }

        public void AddNorthConnector()
        {
            var width = Map_Size.Width;
            var height = Map_Size.Height;
            var yStart = height - 1;

            bool looking = true;
            int lookX = 0, lookY = yStart;
            int x = 0, y = 0;
            for (lookY = yStart; looking && lookY > -1; --lookY)
            {
                for (lookX = 0; looking && lookX < width; ++lookX)
                {
                    var cell = map[lookX, lookY];
                    if (cell >= RoomCell && cell < CorridorCell)
                    {
                        x = lookX;
                        y = lookY;
                        looking = false;
                    }
                }
            }

            //Make sure there was nothing left over
            lPotentialCorridor.Clear();

            for (var fillY = yStart; fillY > y; --fillY)
            {
                if (map[x, fillY] == csMapbuilder.EmptyCell)
                {
                    lPotentialCorridor.Add(new Point(x, fillY));
                }
            }

            if (lPotentialCorridor.Count > 0)
            {
                NorthConnectorIndex = currentCorridorCell;
                corridorTerminatingRooms[currentCorridorCell] = map[x, y];
                Corridor_Build(true);
            }

            NorthConnector = new Point(x, yStart);
        }

        public void AddSouthConnector()
        {
            var width = Map_Size.Width;
            var height = Map_Size.Height;

            bool looking = true;
            int lookX = 0, lookY = 0;
            int x = 0, y = 0;
            for (lookY = 0; looking && lookY < height; ++lookY)
            {
                for (lookX = 0; looking && lookX < width; ++lookX)
                {
                    var cell = map[lookX, lookY];
                    if (cell >= RoomCell && cell < CorridorCell)
                    {
                        x = lookX;
                        y = lookY;
                        looking = false;
                    }
                }
            }

            //Make sure there was nothing left over
            lPotentialCorridor.Clear();

            for (var fillY = 0; fillY < y; ++fillY)
            {
                if (map[x, fillY] == csMapbuilder.EmptyCell)
                {
                    lPotentialCorridor.Add(new Point(x, fillY));
                }
            }

            if (lPotentialCorridor.Count > 0)
            {
                SouthConnectorIndex = currentCorridorCell;
                corridorTerminatingRooms[currentCorridorCell] = map[x, y];
                Corridor_Build(true);
            }

            SouthConnector = new Point(x, 0);
        }

        public void AddWestConnector()
        {
            var width = Map_Size.Width;
            var height = Map_Size.Height;

            bool looking = true;
            int lookX = 0, lookY = 0;
            int x = 0, y = 0;
            for (lookX = 0; looking && lookX < width; ++lookX)
            {
                for (lookY = 0; looking && lookY < height; ++lookY)
                {
                    var cell = map[lookX, lookY];
                    if (cell >= RoomCell && cell < CorridorCell)
                    {
                        x = lookX;
                        y = lookY;
                        looking = false;
                    }
                }
            }

            //Make sure there was nothing left over
            lPotentialCorridor.Clear();

            for (var fillX = 0; fillX < x; ++fillX)
            {
                if (map[fillX, y] == csMapbuilder.EmptyCell)
                {
                    lPotentialCorridor.Add(new Point(fillX, y));
                }
            }

            if (lPotentialCorridor.Count > 0)
            {
                WestConnectorIndex = currentCorridorCell;
                corridorTerminatingRooms[currentCorridorCell] = map[x, y];
                Corridor_Build(true);
            }

            WestConnector = new Point(0, y);
        }

        public void AddEastConnector()
        {
            var width = Map_Size.Width;
            var height = Map_Size.Height;
            var startX = width - 1;

            bool looking = true;
            int lookX = startX, lookY = 0;
            int x = startX, y = 0;
            for (lookX = startX; looking && lookX > -1; --lookX)
            {
                for (lookY = 0; looking && lookY < height; ++lookY)
                {
                    var cell = map[lookX, lookY];
                    if (cell >= RoomCell && cell < CorridorCell)
                    {
                        x = lookX;
                        y = lookY;
                        looking = false;
                    }
                }
            }

            //Make sure there was nothing left over
            lPotentialCorridor.Clear();

            for (var fillX = startX; fillX > x; --fillX)
            {
                if (map[fillX, y] == csMapbuilder.EmptyCell)
                {
                    lPotentialCorridor.Add(new Point(fillX, y));
                }
            }

            if (lPotentialCorridor.Count > 0)
            {
                EastConnectorIndex = currentCorridorCell;
                corridorTerminatingRooms[currentCorridorCell] = map[x, y];
                Corridor_Build(true);
            }

            EastConnector = new Point(startX, y);
        }

        /// <summary>
        /// Get the terminating room for the given corridor id. If the corridor does not terminate in a room
        /// you will get <see cref="csMapbuilder.CorridorCell"/>. This does not mean that it terminates in that
        /// corridor, just that it does not terminate in a room.
        /// </summary>
        /// <param name="corridorId"></param>
        /// <returns></returns>
        public UInt16 GetCorridorTerminatingRoom(UInt16 corridorId)
        {
            if (corridorTerminatingRooms.TryGetValue(corridorId, out var value))
            {
                return (UInt16)(value - csMapbuilder.RoomCell);
            }
            return csMapbuilder.CorridorCell;
        }

        #endregion

        #region room utilities

        /// <summary>
        /// When building a corridor with a room call this function to record the corridor terminating room.
        /// Do this before callig Corridor_Build and Room_Build since the counters get incremented in those functions.
        /// </summary>
        private void RecordCurrentCorridorTerminatingRoom()
        {
            corridorTerminatingRooms[currentCorridorCell] = currentRoomCell;
        }

        /// <summary>
        /// Place a random sized room in the middle of the map
        /// </summary>
        private void PlaceStartRoom()
        {
            rctCurrentRoom = new Rectangle()
            {
                Width = rnd.Next(Room_Min.Width, Room_Max.Width)
                ,
                Height = rnd.Next(Room_Min.Height, Room_Max.Height)
            };
            rctCurrentRoom.Left = Map_Size.Width / 2;
            rctCurrentRoom.Top = Map_Size.Height / 2;
            Room_Build();
        }

        /// <summary>
        /// Place a start room anywhere on the map
        /// </summary>
        private void PlaceStartRooms()
        {
            bool connection = false;
            Point Location = new Point();
            Point Direction = new Point();
            CorridorItemHit CorBuildOutcome;

            while (!connection)
            {

                Clear();

                if (Horizontal)//place a room on the east and west side
                {
                    //west side of room
                    rctCurrentRoom = new Rectangle();
                    rctCurrentRoom.Width = rnd.Next(Room_Min.Width, Room_Max.Width);
                    rctCurrentRoom.Height = rnd.Next(Room_Min.Height, Room_Max.Height);
                    rctCurrentRoom.Top = rnd.Next(0, Map_Size.Height - rctCurrentRoom.Height);
                    rctCurrentRoom.Left = 1;
                    startRoom = rctCurrentRoom;
                    Room_Build();

                    rctCurrentRoom = new Rectangle();
                    rctCurrentRoom.Width = rnd.Next(Room_Min.Width, Room_Max.Width);
                    rctCurrentRoom.Height = rnd.Next(Room_Min.Height, Room_Max.Height);
                    rctCurrentRoom.Top = rnd.Next(0, Map_Size.Height - rctCurrentRoom.Height);
                    rctCurrentRoom.Left = Map_Size.Width - rctCurrentRoom.Width - 2;
                    endRoom = rctCurrentRoom;
                    Room_Build();
                }
                else //place a room on the top and bottom
                {
                    //room at the bottom of the map
                    rctCurrentRoom = new Rectangle()
                    {
                        Width = rnd.Next(Room_Min.Width, Room_Max.Width),
                        Height = rnd.Next(Room_Min.Height, Room_Max.Height)
                    };
                    rctCurrentRoom.Left = rnd.Next(0, Map_Size.Width - rctCurrentRoom.Width);
                    rctCurrentRoom.Top = 1;
                    startRoom = rctCurrentRoom;
                    Room_Build();

                    //at the top of the map
                    rctCurrentRoom = new Rectangle();
                    rctCurrentRoom.Width = rnd.Next(Room_Min.Width, Room_Max.Width);
                    rctCurrentRoom.Height = rnd.Next(Room_Min.Height, Room_Max.Height);
                    rctCurrentRoom.Left = rnd.Next(0, Map_Size.Width - rctCurrentRoom.Width);
                    rctCurrentRoom.Top = Map_Size.Height - rctCurrentRoom.Height - 1;
                    endRoom = rctCurrentRoom;
                    Room_Build();
                }

                if (Corridor_GetStart(out Location, out Direction))
                {
                    CorBuildOutcome = CorridorMake_Straight(ref Location, ref Direction, 100, true);

                    switch (CorBuildOutcome)
                    {
                        case CorridorItemHit.existingroom:
                            //If the starting room is the ending room reverse the list so it always starts at the starting room
                            var checkRoom = csMapbuilder.RoomCell + 1;
                            var firstPoint = lPotentialCorridor[0];
                            var test = firstPoint.x + 1;
                            bool reverse = test < Map_Size.Width && map[test, firstPoint.y] == checkRoom;
                            if (!reverse)
                            {
                                test = firstPoint.x - 1;
                                reverse = test > 0 && map[test, firstPoint.y] == checkRoom;
                                if (!reverse)
                                {
                                    test = firstPoint.y + 1;
                                    reverse = test < Map_Size.Height && map[firstPoint.x, test] == checkRoom;
                                    if (!reverse)
                                    {
                                        test = firstPoint.y - 1;
                                        reverse = test > 0 && map[firstPoint.x, test] == checkRoom;
                                    }
                                }
                            }

                            if (reverse)
                            {
                                lPotentialCorridor.Reverse();
                            }

                            Corridor_Build(true);
                            connection = true;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Make a room off the last point of Corridor, using
        /// CorridorDirection as an indicator of how to offset the room.
        /// The potential room is stored in Room.
        /// </summary>
        private bool Room_AttemptBuildOnCorridor(Point pDirection)
        {
            rctCurrentRoom = new Rectangle()
            {
                Width = rnd.Next(Room_Min.Width, Room_Max.Width)
                    ,
                Height = rnd.Next(Room_Min.Height, Room_Max.Height)
            };

            //startbuilding room from this point
            Point lc = lPotentialCorridor.Last();

            if (pDirection.x == 0) //north/south direction
            {
                rctCurrentRoom.Left = rnd.Next(lc.x - rctCurrentRoom.Width + 1, lc.x);

                if (pDirection.y == 1)
                    rctCurrentRoom.Top = lc.y + 1;//south
                else
                    rctCurrentRoom.Top = lc.y - rctCurrentRoom.Height - 1;//north


            }
            else if (pDirection.y == 0)//east / west direction
            {
                rctCurrentRoom.Top = rnd.Next(lc.y - rctCurrentRoom.Height + 1, lc.y);

                if (pDirection.x == -1)//west
                    rctCurrentRoom.Left = lc.x - rctCurrentRoom.Width;
                else
                    rctCurrentRoom.Left = lc.x + 1;//east
            }

            return Room_Verify();
        }


        /// <summary>
        /// Randomly get a point on the edge of a randomly selected room
        /// </summary>
        /// <param name="Location">Out: Location of point on room edge</param>
        /// <param name="Location">Out: Direction of point</param>
        /// <returns>If Location is legal</returns>
        private void Room_GetEdge(out Point pLocation, out Point pDirection)
        {

            rctCurrentRoom = rctBuiltRooms[rnd.Next(0, rctBuiltRooms.Count())];

            //pick a random point within a room
            //the +1 / -1 on the values are to stop a corner from being chosen
            pLocation = new Point(rnd.Next(rctCurrentRoom.Left + 1, rctCurrentRoom.Right - 1)
                                  , rnd.Next(rctCurrentRoom.Top + 1, rctCurrentRoom.Bottom - 1));


            //get a random direction
            pDirection = directions_straight[rnd.Next(0, directions_straight.GetLength(0))];

            do
            {
                //move in that direction
                pLocation.Offset(pDirection);

                if (!Point_Valid(pLocation.x, pLocation.y))
                    return;

                //until we meet an empty cell
            } while (Point_Get(pLocation.x, pLocation.y) != EmptyCell);

        }

        #endregion

        #region corridor utitlies

        /// <summary>
        /// Randomly get a point on an existing corridor
        /// </summary>
        /// <param name="Location">Out: location of point</param>
        /// <returns>Bool indicating success</returns>
        private void Corridor_GetEdge(out Point pLocation, out Point pDirection)
        {
            List<Point> validdirections = new List<Point>();

            do
            {
                //the modifiers below prevent the first of last point being chosen
                pLocation = lBuilltCorridors[rnd.Next(1, lBuilltCorridors.Count - 1)];

                //attempt to locate all the empy map points around the location
                //using the directions to offset the randomly chosen point
                foreach (Point p in directions_straight)
                    if (Point_Valid(pLocation.x + p.x, pLocation.y + p.y))
                        if (Point_Get(pLocation.x + p.x, pLocation.y + p.y) == EmptyCell)
                            validdirections.Add(p);


            } while (validdirections.Count == 0);

            pDirection = validdirections[rnd.Next(0, validdirections.Count)];
            pLocation.Offset(pDirection);

        }

        /// <summary>
        /// Build the contents of lPotentialCorridor, adding it's points to the builtCorridors
        /// list then empty
        /// </summary>
        private void Corridor_Build(bool walkable)
        {
            UInt16 cellId;
            bool add = true;
            if (walkable)
            {
                cellId = currentCorridorCell++;
            }
            else
            {
                add = AllowOtherCorridors;
                cellId = currentOtherCorridorCell++;
            }

            if (add)
            {
                foreach (Point p in lPotentialCorridor)
                {
                    Point_Set(p.x, p.y, cellId);
                    lBuilltCorridors.Add(p);
                }
            }

            lPotentialCorridor.Clear();
        }

        /// <summary>
        /// Get a starting point for a corridor, randomly choosing between a room and a corridor.
        /// </summary>
        /// <param name="Location">Out: pLocation of point</param>
        /// <param name="Location">Out: pDirection of point</param>
        /// <returns>Bool indicating if location found is OK</returns>
        private bool Corridor_GetStart(out Point pLocation, out Point pDirection)
        {
            rctCurrentRoom = new Rectangle();
            lPotentialCorridor = new List<Point>();

            if (lBuilltCorridors.Count > 0)
            {
                if (rnd.Next(0, 100) >= BuildProb)
                    Room_GetEdge(out pLocation, out pDirection);
                else
                    Corridor_GetEdge(out pLocation, out pDirection);
            }
            else//no corridors present, so build off a room
                Room_GetEdge(out pLocation, out pDirection);

            //finally check the point we've found
            return Corridor_PointTest(pLocation, pDirection) == CorridorItemHit.OK;

        }

        /// <summary>
        /// Attempt to make a corridor, storing it in the lPotentialCorridor list
        /// </summary>
        /// <param name="pStart">Start point of corridor</param>
        /// <param name="pTurns">Number of turns to make</param>
        private CorridorItemHit CorridorMake_Straight(ref Point pStart, ref Point pDirection, int pTurns, bool pPreventBackTracking)
        {

            lPotentialCorridor = new List<Point>();
            lPotentialCorridor.Add(pStart);

            int corridorlength;
            Point startdirection = new Point(pDirection.x, pDirection.y);
            CorridorItemHit outcome;

            while (pTurns > 0)
            {
                pTurns--;

                corridorlength = rnd.Next(Corridor_Min, Corridor_Max);
                //build corridor
                while (corridorlength > 0)
                {
                    corridorlength--;

                    //make a point and offset it
                    pStart.Offset(pDirection);

                    outcome = Corridor_PointTest(pStart, pDirection);
                    if (outcome != CorridorItemHit.OK)
                        return outcome;
                    else
                        lPotentialCorridor.Add(pStart);
                }

                if (pTurns > 1)
                    if (!pPreventBackTracking)
                        pDirection = Direction_Get(pDirection);
                    else
                        pDirection = Direction_Get(pDirection, startdirection);
            }

            return CorridorItemHit.completed;
        }

        /// <summary>
        /// Test the provided point to see if it has empty cells on either side
        /// of it. This is to stop corridors being built adjacent to a room.
        /// </summary>
        /// <param name="pPoint">Point to test</param>
        /// <param name="pDirection">Direction it is moving in</param>
        /// <returns></returns>
        private CorridorItemHit Corridor_PointTest(Point pPoint, Point pDirection)
        {

            if (!Point_Valid(pPoint.x, pPoint.y))//invalid point hit, exit
                return CorridorItemHit.invalid;
            else if (lBuilltCorridors.Contains(pPoint))//in an existing corridor
                return CorridorItemHit.existingcorridor;
            else if (lPotentialCorridor.Contains(pPoint))//hit self
                return CorridorItemHit.self;
            else if (rctCurrentRoom != null && rctCurrentRoom.Contains(pPoint))//the corridors origin room has been reached, exit
                return CorridorItemHit.originroom;
            else
            {
                //is point in a room
                foreach (Rectangle r in rctBuiltRooms)
                    if (r.Contains(pPoint))
                        return CorridorItemHit.existingroom;
            }


            //using the property corridor space, check that number of cells on
            //either side of the point are empty
            foreach (int r in Enumerable.Range(-CorridorSpace, 2 * CorridorSpace + 1).ToList())
            {
                if (pDirection.x == 0)//north or south
                {
                    if (Point_Valid(pPoint.x + r, pPoint.y))
                        if (Point_Get(pPoint.x + r, pPoint.y) != EmptyCell)
                            return CorridorItemHit.tooclose;
                }
                else if (pDirection.y == 0)//east west
                {
                    if (Point_Valid(pPoint.x, pPoint.y + r))
                        if (Point_Get(pPoint.x, pPoint.y + r) != EmptyCell)
                            return CorridorItemHit.tooclose;
                }

            }

            return CorridorItemHit.OK;
        }


        #endregion

        #region direction methods

        /// <summary>
        /// Get a random direction, excluding the opposite of the provided direction to
        /// prevent a corridor going back on it's Build
        /// </summary>
        /// <param name="dir">Current direction</param>
        /// <returns></returns>
        private Point Direction_Get(Point pDir)
        {
            Point NewDir;
            do
            {
                NewDir = directions_straight[rnd.Next(0, directions_straight.GetLength(0))];
            } while (Direction_Reverse(NewDir) == pDir);

            return NewDir;
        }

        /// <summary>
        /// Get a random direction, excluding the provided directions and the opposite of 
        /// the provided direction to prevent a corridor going back on it's self.
        /// 
        /// The parameter pDirExclude is the first direction chosen for a corridor, and
        /// to prevent it from being used will prevent a corridor from going back on 
        /// it'self
        /// </summary>
        /// <param name="dir">Current direction</param>
        /// <param name="pDirectionList">Direction to exclude</param>
        /// <param name="pDirExclude">Direction to exclude</param>
        /// <returns></returns>
        private Point Direction_Get(Point pDir, Point pDirExclude)
        {
            Point NewDir;
            do
            {
                NewDir = directions_straight[rnd.Next(0, directions_straight.GetLength(0))];
            } while (
                        Direction_Reverse(NewDir) == pDir
                         | Direction_Reverse(NewDir) == pDirExclude
                    );


            return NewDir;
        }

        private Point Direction_Reverse(Point pDir)
        {
            return new Point(-pDir.x, -pDir.y);
        }

        #endregion

        #region room test

        /// <summary>
        /// Check if rctCurrentRoom can be built
        /// </summary>
        /// <returns>Bool indicating success</returns>
        private bool Room_Verify()
        {
            //make it one bigger to ensure that testing gives it a border
            rctCurrentRoom.Inflate(RoomDistance, RoomDistance);

            //check it occupies legal, empty coordinates
            for (int x = rctCurrentRoom.Left; x <= rctCurrentRoom.Right; x++)
                for (int y = rctCurrentRoom.Top; y <= rctCurrentRoom.Bottom; y++)
                    if (!Point_Valid(x, y) || Point_Get(x, y) != EmptyCell)
                        return false;

            //check it doesn't encroach onto existing rooms
            foreach (Rectangle r in rctBuiltRooms)
                if (r.IntersectsWith(rctCurrentRoom))
                    return false;

            rctCurrentRoom.Inflate(-RoomDistance, -RoomDistance);

            //check the room is the specified distance away from corridors
            rctCurrentRoom.Inflate(CorridorDistance, CorridorDistance);

            foreach (Point p in lBuilltCorridors)
                if (rctCurrentRoom.Contains(p))
                    return false;

            rctCurrentRoom.Inflate(-CorridorDistance, -CorridorDistance);

            return true;
        }

        /// <summary>
        /// Add the global Room to the rooms collection and draw it on the map
        /// </summary>
        private void Room_Build()
        {
            rctBuiltRooms.Add(rctCurrentRoom);

            for (int x = rctCurrentRoom.Left; x <= rctCurrentRoom.Right; x++)
            {
                for (int y = rctCurrentRoom.Top; y <= rctCurrentRoom.Bottom; y++)
                {
                    map[x, y] = currentRoomCell;
                }
            }

            ++currentRoomCell;

        }

        #endregion

        #region Map Utilities

        /// <summary>
        /// Check if the point falls within the map array range
        /// </summary>
        /// <param name="x">x to test</param>
        /// <param name="y">y to test</param>
        /// <returns>Is point with map array?</returns>
        private Boolean Point_Valid(int x, int y)
        {
            return x >= 0 & x < map.GetLength(0) & y >= 0 & y < map.GetLength(1);
        }

        /// <summary>
        /// Set array point to specified value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="val"></param>
        private void Point_Set(int x, int y, UInt16 val)
        {
            map[x, y] = val;
        }

        /// <summary>
        /// Get the value of the specified point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private UInt16 Point_Get(int x, int y)
        {
            return map[x, y];
        }

        #endregion
    }
}




/**
 * Originally based on code with the following license.
 * 
 * https://github.com/AndyStobirski/RogueLike
 * 

This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org> 
*/