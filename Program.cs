using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipSecuritySystem
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test Values
            List<ColorChip> chipBag = new List<ColorChip>
            {
                new ColorChip(Color.Blue,Color.Purple),
                new ColorChip(Color.Blue,Color.Yellow),
                new ColorChip(Color.Yellow,Color.Green),
                new ColorChip(Color.Yellow, Color.Purple),
                new ColorChip(Color.Red, Color.Orange),
                new ColorChip(Color.Red, Color.Yellow),
                new ColorChip(Color.Purple, Color.Red),
                new ColorChip(Color.Purple, Color.Orange),
                new ColorChip(Color.Purple, Color.Yellow),
                new ColorChip(Color.Purple, Color.Green),
                new ColorChip(Color.Orange, Color.Purple),
                new ColorChip(Color.Orange, Color.Purple),
                new ColorChip(Color.Orange, Color.Yellow),
                new ColorChip(Color.Orange, Color.Red),
                new ColorChip(Color.Orange, Color.Orange),
                new ColorChip(Color.Orange, Color.Orange),
                new ColorChip(Color.Orange, Color.Orange),
                new ColorChip(Color.Orange, Color.Orange),
                new ColorChip(Color.Orange, Color.Red),
                new ColorChip(Color.Blue, Color.Red),
                new ColorChip(Color.Blue, Color.Blue),
                new ColorChip(Color.Red, Color.Yellow),
                new ColorChip(Color.Red, Color.Red),
                new ColorChip(Color.Yellow, Color.Yellow),
                new ColorChip(Color.Yellow, Color.Purple),
                new ColorChip(Color.Purple, Color.Green)
            };
            //If there are is no starting chip or no ending chip, we can throw the error immediately.
            if (chipBag.Where(chp => chp.StartColor == Color.Blue).Count() == 0 || chipBag.Where(chp => chp.EndColor == Color.Green).Count() == 0)
            {
                Console.WriteLine(Constants.ErrorMessage);
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return;
            }
            List<ColorChip> sequence = GetLongestValidPath(chipBag);
            if (sequence.Count > 0)
            {
                Console.WriteLine("Panel can be unlocked!");
                Console.WriteLine("Longest Possible Sequence: ");
                Console.Write("Blue ");
                foreach (ColorChip chip in sequence)
                {
                    Console.Write("["+chip.ToString()+"] ");
                }
                Console.WriteLine("Green");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine(Constants.ErrorMessage);
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
        private static List<ColorChip> GetLongestValidPath(List<ColorChip> chipBag, List<ColorChip> currentCombination = null, List<ColorChip> solution = null)
        {
            //First Iteration Case, add the first sequence for each possible start, as well as begin the process for each next chip.
            //instantiate variables to be used in further recursive calls
            List<ColorChip> nextChips = new List<ColorChip>();
            List<ColorChip> processedChips = new List<ColorChip>();
            if (currentCombination == null)
            {
                currentCombination = new List<ColorChip>();
                solution = new List<ColorChip>();
                List<ColorChip> startingChips = chipBag.Where(chp => chp.StartColor == Color.Blue).ToList();
                foreach (ColorChip chip in startingChips)
                {
                    //if the last added chip is a duplicate of a previously processed chip, do not calculate a sequence we have already calculated.
                    if (processedChips.Where(pch => pch.StartColor == chip.StartColor && pch.EndColor == chip.EndColor).Count() > 0)
                        continue;

                    nextChips = chipBag.Except(startingChips).Where(chp => chp.StartColor == chip.EndColor).ToList();
                    currentCombination.Add(chip);
                    solution = GetLongestValidPath(chipBag.Except(currentCombination).ToList(), currentCombination.ToList(), solution.ToList());
                    processedChips.Add(chip);
                    currentCombination.Clear();
                }
            }
            //every subsiquent call finds each possible path, follows it to the end of each branch
            else
            {
                //Get the next possible chips
                nextChips = chipBag.Where(chp => chp.StartColor == currentCombination.Last().EndColor).ToList();
                //Loop over and check branching combinations for all possible next chips
                foreach (ColorChip nextChip in nextChips)
                {
                    //if the last added chip is a duplicate of a previously processed chip, do not calculate a sequence we have already calculated.
                    if (processedChips.Where(pch => pch.StartColor == nextChip.StartColor && pch.EndColor == nextChip.EndColor).Count() > 0)
                        continue;
                    
                    currentCombination.Add(nextChip);
                    //Detect ending marker chip
                    if (nextChip.EndColor == Color.Green)
                    {
                        //If we have a new longest valid solution, copy it to the solution variable to be returned at the end.
                        if (CheckIfNewLongestSolution(currentCombination, solution))
                        {
                            solution.Clear();
                            solution = currentCombination.ToList();
                        }
                    }
                    else
                    {
                        //When passing the current list of chips, pass it down as a copy  of the bag, excluding the current combination to not accidentally re-use chips
                        //the current combination is also a copy, to keep each branch consistent with its own path
                        solution = GetLongestValidPath(chipBag.Except(currentCombination).ToList(), currentCombination.ToList(), solution.ToList());
                    }
                    processedChips.Add(nextChip);
                    //Remove the current chip from the current combination of chips to be replaced with the next possibility in the loop.
                    currentCombination.Remove(nextChip);
                }
            }
            return solution;
        }
        private static bool CheckIfNewLongestSolution(List<ColorChip> current, List<ColorChip> solution)
        {
            if (current.Count() > solution.Count())
                return true;
            else
                return false;
        }

    }
    
}
