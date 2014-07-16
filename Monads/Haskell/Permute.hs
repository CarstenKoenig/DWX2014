module Permute where

import Data.List (delete, nub)

-- | Liefert die Permutationen einer Liste
--   behandelt die Elemente der Liste als paarweise verschieden
permute :: (Eq a) => [a] -> [[a]]
permute [] = return []
permute xs = do
  x <- nub xs
  xs' <- permute $ delete x xs
  return $ x:xs'

-- | Liefert die Permutationen einer Liste
--   beachtet Gleichheit von Elementen
permute' :: (Eq a) => [a] -> [[a]]
permute' [] = return []
permute' xs = do
  x <- nub xs
  xs' <- permute' $ delete x xs
  return $ x:xs'