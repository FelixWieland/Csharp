using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth {
    public class Labyrinth {
        public char[,] Dungeon {
            get {
                return dungeon;
            }
            set {
                Dungeon = dungeon;
            }
        }
        private char[,] dungeon;
        private int value;

        private List<int> roomPointsY = new List<int>();
        private List<int> roomPointsX = new List<int>();

        private List<int> roomOuterPointsY = new List<int>();
        private List<int> roomOuterPointsX = new List<int>();

        private List<int> choosePointX = new List<int>();
        private List<int> choosePointY = new List<int>();

        public Labyrinth(int level = 0, int Value = 40) {
            dungeon = new char[Value, Value];
            value = Value;
            //Generiere Dungeon
            //TODO: Räume generieren
            for (int y = 0; y < value; y++) {
                for (int x = 0; x < value; x++) {
                    if (y == 0 || y == value - 1) {
                        dungeon[y, x] = 'o';
                    }
                    if (x == 0 || x == value-1) {
                        dungeon[y, x] = 'o';
                    }
                    else {
                        if ((x % 2 == 0) || (y % 2 == 0)) {
                            dungeon[y, x] = 'o';
                        }
                        else {
                            dungeon[y, x] = 'x';
                        }
                    }
                }
            }
            GenerateRoomPoints();
            GenerateRoomFromPoints();
            GenerateMainWays();
            GenerateOtherWays();
            ConnectMainWaysToOtherWays();
            MakeLeftoverPointsToCrossings();
            RemoveUselessDeadends();
            MakeRoomConnectionComplete();
            SetSpawnAndDestinationPoint();
            ResetOuterWall();
        }
        public String log() {
            string loginfo = "";
            loginfo += "Raumpunkte: \n";
            for (int c = 0; c < roomPointsX.Count; c++) {
                loginfo += "Punkt: " + "Y: " + roomPointsY[c].ToString() + " X: " + roomPointsY[c].ToString() + "\n";
            }
            loginfo += "Äusere Raumpunkte: \n";
            for (int c = 0; c < roomOuterPointsX.Count; c++) {
                loginfo += "OuterPunkt: " + "Y: " + roomOuterPointsY[c].ToString() + " X: " + roomOuterPointsY[c].ToString() + "\n";
            }
            loginfo += "\n Wirkliche Raumpunkte: \n";
            for (int c = 0; c < choosePointX.Count; c++) {
                loginfo += "RealPunkt: " + "Y: " + choosePointY[c].ToString() + " X: " + choosePointX[c].ToString() + "\n";
            }
            return loginfo;
        }
        
        public void ResetOuterWall() {
            for(int y = 0; y < value; y++) {
                for (int x = 0; x < value; x++) {
                    if (y == 0 || x == value - 1 || x == 0 || y == value -1) {
                        Dungeon[y, x] = 'o';
                    }
                }
            }
        }

        public void GenerateRoomPoints() {
            Random rnd = new Random();
            int myval = 6;
            int maybePoint = value / 2 -1;
            int count = 0;
            bool setroom = false;
            int choosePoint = rnd.Next(0, value * value / myval);
            for (int y = 0; y < value; y++) {
                
                for (int x = 0; x < value; x++) {
                    count++;
                    if (setroom == false) {
                        if (count == choosePoint) {
                            try {
                                if (Dungeon[y - 1, x] == 'x') {
                                    Dungeon[y - 1, x] = 'w';
                                    roomPointsY.Add(y-1);
                                    roomPointsX.Add(x);
                                }
                                if (Dungeon[y - 1, x - 1] == 'x') {
                                    Dungeon[y - 1, x - 1] = 'w';
                                    roomPointsY.Add(y - 1);
                                    roomPointsX.Add(x -1);
                                }
                                if (Dungeon[y, x - 1] == 'x') {
                                    Dungeon[y, x - 1] = 'w';
                                    roomPointsY.Add(y);
                                    roomPointsX.Add(x -1);
                                }
                                if(Dungeon[y, x] == 'x') {
                                    Dungeon[y, x] = 'w';
                                    roomPointsY.Add(y);
                                    roomPointsX.Add(x);
                                }
                                setroom = true;
                            }
                            catch {
                                choosePoint++;
                            }
                        }
                        
                    }
                    if(value*value/myval == count) {
                        choosePoint = rnd.Next(0, value * value / myval);
                        count = 0;
                        setroom = false;
                    }
                    
                }
            }
        }
        public void GenerateRoomFromPoints() {
            int checkRoundHouse(int y, int x, char direction) {
                char[,] copy = Dungeon;
                bool possible = true;
                int count = 0;
                switch ((direction.ToString().ToUpper().ToCharArray())[0]) {
                    case 'U':
                        while (possible == true) {
                            try {
                                if (copy[y - count, x] == x) {
                                    count += 2;
                                }
                                else {
                                    return count;
                                }
                            }
                            catch {
                                return count;
                            }
                        }
                        break;
                    case 'D':
                        while (possible == true) {
                            try {
                                if (copy[y + count, x] == x) {
                                    count += 2;
                                }
                                else {
                                    return count;
                                }
                            }
                            catch {
                                return count;
                            }
                        }
                        break;
                    case 'L':
                        while (possible == true) {
                            try {
                                if (copy[y, x - count] == x) {
                                    count += 2;
                                }
                                else {
                                    return count;
                                }
                            }
                            catch {
                                return count;
                            }
                        }
                        break;
                    case 'R':
                        while (possible == true) {
                            try {
                                if (copy[y, x + count] == x) {
                                    count += 2;
                                }
                                else {
                                    return count;
                                }
                            }
                            catch {
                                return count;
                            }
                        }
                        break;
                    default:
                        break;
                }
                return 1;
            }
            void expand(char direction, int walk1, int walk2, int posy, int posx) {
                switch ((direction.ToString().ToUpper().ToCharArray())[0]) {
                    case 'U':
                        roomOuterPointsX.Add(0);
                        roomOuterPointsY.Add(0);
                        for (int range = 0; range <= walk1; range++) {
                            for (int range2 = 0; range2 <= walk2; range2++) {
                                Dungeon[posy - range, posx + range2] = 'w';
                            }
                            Dungeon[posy - range, posx] = 'w';
                            roomOuterPointsX[roomOuterPointsX.Count - 1] = posx*2;
                            roomOuterPointsY[roomOuterPointsY.Count - 1] = (posy-range)*2;
                        }
                        break;
                    case 'D':
                        roomOuterPointsX.Add(0);
                        roomOuterPointsY.Add(0);
                        for (int range = 0; range <= walk1; range++) {
                            for (int range2 = 0; range2 <= walk2; range2++) {
                                Dungeon[posy + range, posx + range2] = 'w';
                            }
                            Dungeon[posy + range, posx] = 'w';
                            roomOuterPointsX[roomOuterPointsX.Count - 1] = posx;
                            roomOuterPointsY[roomOuterPointsY.Count - 1] = posy + range;
                        }
                        break;
                    case 'L':
                        roomOuterPointsX.Add(0);
                        roomOuterPointsY.Add(0);
                        for (int range = 0; range <= walk1; range++) {
                            for (int range2 = 0; range2 <= walk2; range2++) {
                                Dungeon[posy - range2, posx - range] = 'w';
                            }
                            Dungeon[posy, posx - range] = 'w';
                            roomOuterPointsX[roomOuterPointsX.Count - 1] = posx - range;
                            roomOuterPointsY[roomOuterPointsY.Count - 1] = posy;
                        }
                        break;
                    case 'R':
                        roomOuterPointsX.Add(0);
                        roomOuterPointsY.Add(0);
                        for (int range = 0; range <= walk1; range++) {
                            for (int range2 = 0; range2 <= walk2; range2++) {
                                Dungeon[posy + range2, posx - range] = 'w';
                            }
                            Dungeon[posy, posx + range] = 'w';
                            roomOuterPointsX[roomOuterPointsX.Count - 1] = posx + range;
                            roomOuterPointsY[roomOuterPointsY.Count - 1] = posy;
                        }
                        break;
                    default:
                        break;
                }
            }

            int maxSize = 4;
            int minSize = 2;
            bool onetime = false;

            for (int c = 0; c < roomPointsX.Count; c++) {
                {
                int maxExpandUp = checkRoundHouse(roomPointsY[c], roomPointsX[c], 'u');
                int maxExpandDown = checkRoundHouse(roomPointsY[c], roomPointsX[c], 'd');
                int maxExpandLeft = checkRoundHouse(roomPointsY[c], roomPointsX[c], 'l');
                int maxExpandRight = checkRoundHouse(roomPointsY[c], roomPointsX[c], 'r');
                
                int chooseRoomSize = 0;
                
                    if (maxExpandUp >= maxSize) chooseRoomSize += maxSize;
                    else if (maxExpandUp >= minSize && maxExpandUp < maxSize) chooseRoomSize += minSize;

                    if (maxExpandDown >= maxSize) chooseRoomSize += maxSize;
                    else if (maxExpandDown >= minSize && maxExpandDown < maxSize) chooseRoomSize += minSize;

                    if (maxExpandLeft >= maxSize) chooseRoomSize += maxSize;
                    else if (maxExpandLeft >= minSize && maxExpandLeft < maxSize) chooseRoomSize += minSize;

                    if (maxExpandRight >= maxSize) chooseRoomSize += maxSize;
                    else if (maxExpandRight >= minSize && maxExpandRight < maxSize) chooseRoomSize += minSize;
                }
                if (onetime == false) {
                    try {
                        expand('d', 2, 2, roomPointsY[c], roomPointsX[c]);
                    }
                    catch {
                        //none
                    }
                    try {
                        expand('l', 2, 2, roomPointsY[c], roomPointsX[c]);
                    }
                    catch {

                    }
                    try {
                        expand('r', 2, 2, roomPointsY[c], roomPointsX[c]);
                    }
                    catch {

                    }
                    try {
                        expand('u', 2, 2, roomPointsY[c], roomPointsX[c]);
                    }
                    catch {

                    }
                    onetime = false;
                }
            }
        }

        public void GenerateMainWays() {
            List<int> walkPointsY = new List<int>();
            List<int> walkPointsX = new List<int>();
            for (int i = 0; i < roomOuterPointsX.Count; i++) {
                try {
                    if (Dungeon[roomOuterPointsY[i], roomOuterPointsX[i]] == 'w') {
                        choosePointY.Add(roomOuterPointsY[i]);
                        choosePointX.Add(roomOuterPointsX[i]);
                    }
                }
                catch {
                    //none
                }
            }

            bool checkNext(int y, int x, char direction) {
                switch ((direction.ToString().ToUpper().ToCharArray())[0]) {
                    case 'U':
                        try {
                            if (Dungeon[y - 2, x] == 'x') {
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    case 'D':
                        try {
                            if (Dungeon[y + 2, x] == 'x') {
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    case 'L':
                        try {
                            if (Dungeon[y, x - 2] == 'x') {
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    case 'R':
                        try {
                            if (Dungeon[y, x + 2] == 'x') {
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    default:
                        return false;
                }
            }
            bool walkNext(int y, int x, char direction, char wayArt = 't') {
                switch ((direction.ToString().ToUpper().ToCharArray())[0]) {
                    case 'U':
                        try {
                            if (Dungeon[y - 2, x] == 'x') {
                                for (int i = 0; i < 3; i++) {
                                    Dungeon[y - i, x] = wayArt;
                                }
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    case 'D':
                        try {
                            if (Dungeon[y + 2, x] == 'x') {
                                for (int i = 0; i < 3; i++) {
                                    Dungeon[y + i, x] = wayArt;
                                }
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    case 'L':
                        try {
                            if (Dungeon[y, x - 2] == 'x') {
                                for (int i = 0; i < 3; i++) {
                                    Dungeon[y, x - i] = wayArt;
                                }
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    case 'R':
                        try {
                            if (Dungeon[y, x + 2] == 'x') {
                                for (int i = 0; i < 3; i++) {
                                    Dungeon[y, x + i] = wayArt;
                                }
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    default:
                        return false;
                }
            }

            for (int i = 0; i < choosePointX.Count; i++) {
                //UP
                if(checkNext(choosePointY[i], choosePointX[i], 'u') == true) {
                    walkPointsY.Add(choosePointY[i]-2);
                    walkPointsX.Add(choosePointX[i]);
                }
                //DOWN
                if (checkNext(choosePointY[i], choosePointX[i], 'd') == true) {
                    walkPointsY.Add(choosePointY[i]+2);
                    walkPointsX.Add(choosePointX[i]);
                }
                //LEFT
                if (checkNext(choosePointY[i], choosePointX[i], 'l') == true) {
                    walkPointsY.Add(choosePointY[i]);
                    walkPointsX.Add(choosePointX[i]-2);
                }
                //RIGHT
                if (checkNext(choosePointY[i], choosePointX[i], 'r') == true) {
                    walkPointsY.Add(choosePointY[i]);
                    walkPointsX.Add(choosePointX[i]+2);
                }
            }
            if(walkPointsX.Count == 0) {
                //LevelNeuGenerieren
            }
            
            
            Random rnd = new Random();
            for (int i = 0; i < walkPointsX.Count; i++) {
                //WalkDown
                int y = 0;
                int x = 0;
                if (checkNext(walkPointsY[i], walkPointsX[i], 'u')) {
                    walkNext(walkPointsY[i], walkPointsX[i], 'u');
                    y = walkPointsY[i] - 2;
                    x = walkPointsX[i];
                }
                else if (checkNext(walkPointsY[i], walkPointsX[i], 'd')) {
                    walkNext(walkPointsY[i], walkPointsX[i], 'd');
                    y = walkPointsY[i] + 2;
                    x = walkPointsX[i];
                }
                else if (checkNext(walkPointsY[i], walkPointsX[i], 'l')) {
                    walkNext(walkPointsY[i], walkPointsX[i], 'l');
                    y = walkPointsY[i];
                    x = walkPointsX[i] - 2;
                }
                else if (checkNext(walkPointsY[i], walkPointsX[i], 'r')) {
                    walkNext(walkPointsY[i], walkPointsX[i], 'r');
                    y = walkPointsY[i];
                    x = walkPointsX[i] + 2;
                }
                else {
                    continue;
                }

                int counter = 0;

                List<int> chooseFromThis = new List<int>();
                chooseFromThis = new List<int>();
                chooseFromThis.Add(1);
                chooseFromThis.Add(2);
                chooseFromThis.Add(3);
                chooseFromThis.Add(4);

                while (checkNext(y, x, 'u') == true || checkNext(y, x, 'd') == true || checkNext(y, x, 'l') == true || checkNext(y, x, 'r') == true) {
                    if (counter >= 500) {
                        break;
                    }
                    int dir = rnd.Next(0, chooseFromThis.Count-1);
                    
                    switch (chooseFromThis[dir]) {
                        case 1:
                            if(checkNext(y, x, 'u')) {
                                walkNext(y, x, 'u');
                                y = y - 2;
                                chooseFromThis = new List<int>();
                                chooseFromThis.Add(2);
                                chooseFromThis.Add(3);
                                chooseFromThis.Add(4);
                            }
                            break;
                        case 2:
                            if (checkNext(y, x, 'd')) {
                                walkNext(y, x, 'd');
                                y = y + 2;
                                chooseFromThis = new List<int>();
                                chooseFromThis.Add(1);
                                chooseFromThis.Add(3);
                                chooseFromThis.Add(4);
                            }
                            break;
                        case 3:
                            if (checkNext(y, x, 'l')) {
                                walkNext(y, x, 'l');
                                x = x - 2;
                                chooseFromThis = new List<int>();
                                chooseFromThis.Add(1);
                                chooseFromThis.Add(2);
                                chooseFromThis.Add(4);
                            }
                            break;
                        case 4:
                            if (checkNext(y, x, 'r')) {
                                walkNext(y, x, 'r');
                                x = x + 2;
                                chooseFromThis = new List<int>();
                                chooseFromThis.Add(1);
                                chooseFromThis.Add(2);
                                chooseFromThis.Add(3);
                            }
                            break;
                        default:
                            break;
                    }
                    counter++;
                    
                }
            }
            //Generiere Wege von den Mitten der Räumen
        }
        public void GenerateOtherWays() {
            bool checkNext(int y, int x, char direction) {
                switch ((direction.ToString().ToUpper().ToCharArray())[0]) {
                    case 'U':
                        try {
                            if (Dungeon[y - 2, x] == 'x') {
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    case 'D':
                        try {
                            if (Dungeon[y + 2, x] == 'x') {
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    case 'L':
                        try {
                            if (Dungeon[y, x - 2] == 'x') {
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    case 'R':
                        try {
                            if (Dungeon[y, x + 2] == 'x') {
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    default:
                        return false;
                }
            }
            bool walkNext(int y, int x, char direction, char wayArt = 't') {
                switch ((direction.ToString().ToUpper().ToCharArray())[0]) {
                    case 'U':
                        try {
                            if (Dungeon[y - 2, x] == 'x') {
                                for (int i = 0; i < 3; i++) {
                                    Dungeon[y - i, x] = wayArt;
                                }
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    case 'D':
                        try {
                            if (Dungeon[y + 2, x] == 'x') {
                                for (int i = 0; i < 3; i++) {
                                    Dungeon[y + i, x] = wayArt;
                                }
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    case 'L':
                        try {
                            if (Dungeon[y, x - 2] == 'x') {
                                for (int i = 0; i < 3; i++) {
                                    Dungeon[y, x - i] = wayArt;
                                }
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    case 'R':
                        try {
                            if (Dungeon[y, x + 2] == 'x') {
                                for (int i = 0; i < 3; i++) {
                                    Dungeon[y, x + i] = wayArt;
                                }
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        catch {
                            return false;
                        }
                    default:
                        return false;
                }
            }

            for (int y = 0; y < value; y++) {
                for(int x = 0; x < value; x++) {
                    if(Dungeon[y,x] == 'x') {
                        int y2 = y;
                        int x2 = x;
                        while(checkNext(y2, x2, 'u') || checkNext(y2, x2, 'd') || checkNext(y2, x2, 'l') || checkNext(y2, x2, 'r')) {
                            if (checkNext(y2, x2, 'u')) {
                                walkNext(y2, x2, 'u', 'u');
                                y2 -= 2;
                            }
                            if (checkNext(y2, x2, 'd')) {
                                walkNext(y2, x2, 'd', 'u');
                                y2 += 2;
                            }
                            if (checkNext(y2, x2, 'l')) {
                                walkNext(y2, x2, 'l', 'u');
                                x2 -= 2;
                            }
                            if (checkNext(y2, x2, 'r')) {
                                walkNext(y2, x2, 'r', 'u');
                                x2 += 2;
                            }
                        }
                    }
                }
            }
        }

        private void ConnectMainWaysToOtherWays() {

            bool modelCheck(int y, int x, char directory) {
                int countVar = 0;
                //TODO: prüfe ob setblock schon ein Weg ist !!
                try {
                    switch ((directory.ToString().ToUpper().ToCharArray())[0]) {
                        case 'U':
                            y = y - 2;
                            x = x - 1;
                            break;
                        case 'D':
                            x = x - 1;
                            break;
                        case 'L':
                            y = y - 1;
                            break;
                        case 'R':
                            y = y - 1;
                            x = x - 2;
                            break;
                        default:
                            break;
                    }
                    for (int y1 = 0; y1 < 3; y1++) {
                        for (int x1 = 0; x1 < 3; x1++) {
                            if (Dungeon[y + y1, x + x1] == 'o') {
                                countVar++;
                            }
                        }
                    }
                }
                catch {

                }
                
                if(countVar >= 5) {
                    return true;
                }
                else {
                    return false;
                }
            }
            bool walkOne(int y, int x, char directory) {
                try {
                    switch ((directory.ToString().ToUpper().ToCharArray())[0]) {
                        case 'U':
                            Dungeon[y - 1, x] = 'c';
                            return true;
                        case 'D':
                            Dungeon[y + 1, x] = 'c';
                            return true;
                        case 'L':
                            Dungeon[y, x - 1] = 'c';
                            return true;
                        case 'R':
                            Dungeon[y, x + 1] = 'c';
                            return true;
                        default:
                            return false;
                    }
                }
                catch {
                    return false;
                }
            }

            for (int y = 0; y < value; y++) {
                for (int x = 0; x < value; x++) {
                    if (Dungeon[y, x] == 't') {
                        if (modelCheck(y, x, 'u')) {
                            walkOne(y, x, 'u');
                        }
                        if (modelCheck(y, x, 'd')) {
                            walkOne(y, x, 'd');
                        }
                        if (modelCheck(y, x, 'l')) {
                            walkOne(y, x, 'l');
                        }
                        if (modelCheck(y, x, 'r')) {
                            walkOne(y, x, 'r');
                        }
                    }
                }
            }
        }
        private void MakeLeftoverPointsToCrossings() {

            for (int y = 0; y < value; y++) {
                for (int x = 0; x < value; x++) {
                    if(Dungeon[y,x] == 'x') {
                        int count = 0;
                        int y2 = y - 1;
                        int x2 = x - 1;

                        for(int i = 0; i < 3; i++) {
                            for (int a = 0; a < 3; a++) {
                                if(Dungeon[y2 + i, x2 + a] != 'o') {
                                    count++;
                                }
                            }
                        }

                        if(count >= 2) {
                            Dungeon[y,x] = 'v';
                        }
                        else {
                            Dungeon[y, x] = 'v';

                            Dungeon[y-1, x] = 'v';
                            Dungeon[y+1, x] = 'v';
                            Dungeon[y, x-1] = 'v';
                            Dungeon[y, x+1] = 'v';
                        }

                    }
                }
            }
        }

        private void RemoveUselessDeadends() {

            for (int y = 0; y < value; y++) {
                for (int x = 0; x < value; x++) {
                    if (Dungeon[y,x] != 'o') {
                        int y2 = y - 1;
                        int x2 = x - 1;
                        int count = 0;

                        for(int i = 0; i < 3; i++) {
                            for(int a = 0; a < 3; a++) {
                                try {
                                    if (Dungeon[y2 + i, x2 + a] != 'o') {
                                        count++;
                                    }
                                }
                                catch {

                                }
                            }
                        }

                        if (count <= 2) {
                            try {
                                if (Dungeon[y - 1, x] != 'o') { //UP
                                    Dungeon[y + 1, x] = 'c';
                                }
                                else if (Dungeon[y + 1, x] != 'o') { //DOWN
                                    Dungeon[y - 1, x] = 'c';
                                }
                                else if (Dungeon[y, x - 1] != 'o') { //LEFT
                                    Dungeon[y, x + 1] = 'c';
                                }
                                else if (Dungeon[y, x + 1] != 'o') { //RIGHT
                                    Dungeon[y, x - 1] = 'c';
                                }
                            }
                            catch {

                            }
                        }
                    }
                }
            }
        }
        private void MakeRoomConnectionComplete() {

            bool checkSide(int y, int x, char direction) {
                try {
                    switch ((direction.ToString().ToUpper().ToCharArray())[0]) {
                        case 'U':
                            if (Dungeon[y + 1, x] == 'o') {
                                return true;
                            }
                            else {
                                return false;
                            }
                        case 'D':
                            if (Dungeon[y - 1, x] == 'o') {
                                return true;
                            }
                            else {
                                return false;
                            }
                        case 'L':
                            if (Dungeon[y, x - 1] == 'o') {
                                return true;
                            }
                            else {
                                return false;
                            }
                        case 'R':
                            if (Dungeon[y, x + 1] == 'o') {
                                return true;
                            }
                            else {
                                return false;
                            }
                        default:
                            return false;
                            break;
                    }
                }
                catch {
                    return false;
                }
                
            }

            for(int i = 0; i < choosePointX.Count; i++) {
                if (checkSide(choosePointY[i], choosePointX[i], 'u')) {
                    try {
                        Dungeon[choosePointY[i] + 1, choosePointX[i]] = 'c';
                    }
                    catch {

                    }
                }
                if (checkSide(choosePointY[i], choosePointX[i], 'd')) {
                    try {
                        Dungeon[choosePointY[i] - 1, choosePointX[i]] = 'c';
                    }
                    catch {

                    }
                }
                if (checkSide(choosePointY[i], choosePointX[i], 'l')) {
                    try {
                        Dungeon[choosePointY[i], choosePointX[i] - 1] = 'c';
                    }
                    catch {

                    }
                }
                if (checkSide(choosePointY[i], choosePointX[i], 'r')) {
                    try {
                        Dungeon[choosePointY[i], choosePointX[i] + 1] = 'c';
                    }
                    catch {

                    }
                }
            }
        }

        private void SetSpawnAndDestinationPoint() {
            try {
                //Spawn
                Dungeon[roomPointsY[0], roomPointsX[0]] = 's';
                //Destination
                Dungeon[roomPointsY[roomPointsY.Count - 1], roomPointsX[roomPointsX.Count - 1]] = 'e';
            }
            catch {

            }
        }
    }
}
