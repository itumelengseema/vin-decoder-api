namespace VinDecoder.Api.Services
{
    public class VinCheckDigitService
    {
        private readonly Dictionary<char, int> _characterValues = new()
        {
            {'A',1},
            {'B',2},
            {'C',3},
            {'D',4},
            {'E',5},
            {'F',6},
            {'G',7},
            {'H',8},
            {'J',1},
            {'K',2},
            {'L',3},
            {'M',4},
            {'N',5},
            {'P',7},
            {'R',9},
            {'S',2},
            {'T',3},
            {'U',4},
            {'V',5},
            {'W',6},
            {'X',7},
            {'Y',8},
            {'Z',9},
        };

        private readonly int[] _weights =
        {
            8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2
        };
        private int GetCharacterValue(char character)
        {
            if (char.IsDigit(character))
            {
                return int.Parse(character.ToString());
            }
            return _characterValues[character];
        }

        public char CalculateCheckDigit(string vin)
        {
            int total = 0;

            for (int i = 0; i < vin.Length; i++)
            {
                int value = GetCharacterValue(vin[i]);
                int weight = _weights[i];
                total = total + (value * weight);
            }

            int remainder = total % 11;
            if (remainder == 10)
            {
                return 'X';}

            return remainder.ToString()[0];
        }

        public bool IsValid(string vin)
        {
            char actualCheckDigit = vin[8];
            char calculatedCheckDigit = CalculateCheckDigit(vin);

            if (actualCheckDigit == calculatedCheckDigit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RequiresCheckDigitValidation(string vin)
        {
            char firstCharacter = vin[0];
            
            return firstCharacter is '1' or '2' or '3' or '4' or '5';
        }
    }
}