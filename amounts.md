# Amount Organization Specification for Wasabi Wallet 2.0

## Target User

Peter McCormack

## Objectives

- Avoid homogenic coin distribution.
- Avoid UTXO bloat.
- Significantly lower minimum denominations compared to [WW1.](https://github.com/nopara73/ZeroLink/)
- Thrive for not generating non-mixed changes.
- Thrive for faster mixing.
- Thrive for more blockspace efficient mixing.

## Approach

Since this amount organization will be used in Wasabi Wallet 2.0, a sensible strategy is to start out with the current amount organization of WW1 and step by step improve upon that.

### Setup

In order to verify our reasoning and intuitions we [sampled 8000 input amounts](./AmountOrganization/AmountOrganization/Sample.txt) from Wasabi Wallet 1.0 coinjoins around December 2020, those have not been mixed before.  
We'll use in various ways throughout this document, mostly from code, which source can be found under [AmountOrganization solution.](./AmountOrganization)

### 0. Skeleton of WWI's Amount Organization

To start out, we took 50 inputs randomly out of our sample and created 40 random groupings - users - out of it. Then implemented WW1's mixing technique and got the following example output.

How does WW1's mixing technique work? It takes the amounts and loops them through powers of 2 denominations in ascending order. It creates outputs for every denomination level the user can still participate in. If it can't it creates a change output.

We shall also note that this example dismisses many variables, most importantly the minimum denomination Wasabi imposes and the network fees. We'll introduce them later on.

```
Number of users:        40
Number of inputs:       50
Number of outputs:      937

There are 40 occurrences of 0.00000001 BTC output.
There are 40 occurrences of 0.00000002 BTC output.
There are 40 occurrences of 0.00000004 BTC output.
There are 40 occurrences of 0.00000008 BTC output.
There are 40 occurrences of 0.00000016 BTC output.
There are 40 occurrences of 0.00000032 BTC output.
There are 40 occurrences of 0.00000064 BTC output.
There are 40 occurrences of 0.00000128 BTC output.
There are 40 occurrences of 0.00000256 BTC output.
There are 40 occurrences of 0.00000512 BTC output.
There are 40 occurrences of 0.00001024 BTC output.
There are 40 occurrences of 0.00002048 BTC output.
There are 40 occurrences of 0.00004096 BTC output.
There are 40 occurrences of 0.00008192 BTC output.
There are 40 occurrences of 0.00016384 BTC output.
There are 40 occurrences of 0.00032768 BTC output.
There are 40 occurrences of 0.00065536 BTC output.
There are 1 occurrences of 0.00091212 BTC output.
There are 40 occurrences of 0.00131072 BTC output.
There are 1 occurrences of 0.00133484 BTC output.
There are 1 occurrences of 0.00139596 BTC output.
There are 1 occurrences of 0.00160831 BTC output.
There are 1 occurrences of 0.00204713 BTC output.
There are 1 occurrences of 0.00232849 BTC output.
There are 1 occurrences of 0.00250764 BTC output.
There are 1 occurrences of 0.00251425 BTC output.
There are 40 occurrences of 0.00262144 BTC output.
There are 1 occurrences of 0.00319055 BTC output.
There are 1 occurrences of 0.00501425 BTC output.
There are 34 occurrences of 0.00524288 BTC output.
There are 1 occurrences of 0.00628344 BTC output.
There are 1 occurrences of 0.00766987 BTC output.
There are 1 occurrences of 0.008003 BTC output.
There are 1 occurrences of 0.00889539 BTC output.
There are 31 occurrences of 0.01048576 BTC output.
There are 1 occurrences of 0.01064874 BTC output.
There are 1 occurrences of 0.01079982 BTC output.
There are 1 occurrences of 0.01080556 BTC output.
There are 1 occurrences of 0.01237489 BTC output.
There are 1 occurrences of 0.01273657 BTC output.
There are 1 occurrences of 0.01345343 BTC output.
There are 1 occurrences of 0.0138582 BTC output.
There are 1 occurrences of 0.01415412 BTC output.
There are 1 occurrences of 0.0147032 BTC output.
There are 1 occurrences of 0.01599117 BTC output.
There are 1 occurrences of 0.01711393 BTC output.
There are 1 occurrences of 0.01849849 BTC output.
There are 28 occurrences of 0.02097152 BTC output.
There are 1 occurrences of 0.02611393 BTC output.
There are 1 occurrences of 0.03132254 BTC output.
There are 1 occurrences of 0.031777 BTC output.
There are 1 occurrences of 0.0360725 BTC output.
There are 1 occurrences of 0.0384578 BTC output.
There are 19 occurrences of 0.04194304 BTC output.
There are 1 occurrences of 0.05586343 BTC output.
There are 1 occurrences of 0.06800123 BTC output.
There are 1 occurrences of 0.06908003 BTC output.
There are 12 occurrences of 0.08388608 BTC output.
There are 1 occurrences of 0.10004366 BTC output.
There are 1 occurrences of 0.10557624 BTC output.
There are 1 occurrences of 0.10746535 BTC output.
There are 1 occurrences of 0.11578316 BTC output.
There are 1 occurrences of 0.13324562 BTC output.
There are 1 occurrences of 0.1543588 BTC output.
There are 7 occurrences of 0.16777216 BTC output.
There are 4 occurrences of 0.33554432 BTC output.
There are 2 occurrences of 0.67108864 BTC output.
```