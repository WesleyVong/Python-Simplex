import numpy as np

def pivot(pivot_row, pivot_column, array, r_list, t_list):
    array_shape = array.shape
    tmp_array = np.zeros(array_shape)
    row = 0
    column = 0

    tmp = r_list[pivot_column]
    r_list[pivot_column] = t_list[pivot_row]
    t_list[pivot_row] = tmp
    
    while (row != array_shape[0]):
        if row == pivot_row and column == pivot_column:
            tmp_array[row,column] = 1/array[row,column] 
        elif row == pivot_row:
            tmp_array[row,column] = array[row,column]/array[pivot_row,pivot_column]
        elif column == pivot_column:
            tmp_array[row,column] = array[row,column]/array[pivot_row,pivot_column] * -1
        else:
            tmp_array[row,column] = array[row,column] - (array[pivot_row,column] * array[row,pivot_column])/array[pivot_row,pivot_column]
        if column != array_shape[1]-1:
            column = column + 1
        else:
            row = row + 1
            column = 0
    array = tmp_array
    return array, r_list, t_list

def split(array):
    #Turns the full matrix into a b and c arrays
    a_array = array[:array.shape[0]-1,:array.shape[1]-1]
    b_array = array[:array.shape[0]-1,array.shape[1]-1]
    c_array = array[array.shape[0]-1,:array.shape[1]-1]
    return a_array,b_array,c_array

def dual(a_array,b_array,c_array,r_list,t_list):
    
    #Transposes a
    a_array = a_array.T
    
    #swaps b and c
    tmp = b_array
    b_array = np.array([np.negative(c_array)])
    c_array = np.array([tmp])
    array = np.append(a_array, np.negative(c_array), axis = 0)
    array = np.append(array, np.append(b_array.swapaxes(0,1),[[0]], axis = 0), axis = 1)
    tmp = r_list
    r_list = t_list
    t_list = tmp
    a_array,b_array,c_array = split(array)
    print (array)
    return array,a_array,b_array,c_array,r_list,t_list

def simplex(array, maximize = "max"):
    noprint = False

    #Prep array by splitting
    r_list = []
    t_list = []

    a_array, b_array, c_array = split(array)
    
    if maximize == "max":
        for i in range(c_array.shape[0]):
            r_list.append("x" + str(i+1))
        for j in range(b_array.shape[0]):
            t_list.append("y" + str(j+1))
    if maximize == "min":
        for i in range(c_array.shape[0]):
            r_list.append("y" + str(i+1))
        for j in range(b_array.shape[0]):
            t_list.append("x" + str(j+1))
        array,a_array,b_array,c_array,r_list,t_list = dual(a_array,b_array,c_array,r_list,t_list)

        
    while (True):
        ratios_list = []
        tag_list = []
        case = 1
        proceed = False
        check = 0
        b_neg = 0
        c_neg = 0
        
        for term in b_array:
            if term < 0:
                b_neg = 1
        for term in c_array:
            if term < 0:
                c_neg = 1

        if b_neg == 0 and c_neg == 1:
            case = 1
        if b_neg == 1 and c_neg == 1:
            case = 2
        if b_neg == 1 and c_neg == 0:
            case = 3

        #For case 1 problems        
        if case == 1:
            for j in range(c_array.shape[0]):
                if c_array[j] < 0:
                    check = check + 1
                    for i in range(a_array.shape[0]):
                        if a_array[i][j] > 0:
                           ratios_list.append(b_array[i]/a_array[i][j])
                           tag_list.append(i)
                    break
            if len(ratios_list) == 0 and check > 0:
                noprint = True
                print ("unbounded feasible")
                break
            elif len(ratios_list) == 0:
                break
        #For case 2 problems
        if case == 2:
            for i in range(b_array.shape[0]):
                if b_array[i] < 0:
                    for j in range(a_array.shape[1]):
                        if a_array[i][j] < 0:
                            for i in range(a_array.shape[0]):
                                if a_array[i][j] != 0 and b_array[i] * a_array[i][j] >= 0:
                                   ratios_list.append(b_array[i]/a_array[i][j])
                                   tag_list.append(i)
                            if len(ratios_list) > 0:
                                check = 1
                                break
            if check == 0:
                noprint = True
                print ("infeasible")
                break
        #For Dual Simplex case 1
        if case == 3:
            for i in range(b_array.shape[0]):
                if b_array[i] < 0:
                    for j in range(a_array.shape[1]):
                        if a_array[i][j] < 0:
                           ratios_list.append(c_array[j]/a_array[i][j])
                           tag_list.append(j)
                    break

                                
        for ratio in range(len(ratios_list)):
            if ratios_list[ratio] == min(ratios_list):
                array,r_list,t_list = pivot(tag_list[ratio],j, array, r_list, t_list)
                a_array,b_array,c_array = split(array)
                break
    if noprint == False:
        print("value: ", array[array.shape[0]-1,array.shape[1]-1])
        for i in range(len(t_list)):
            if t_list[i].find("y") == 0:
                print(t_list[i], "0")
            else:
                print(t_list[i], b_array[i])
        for j in range(len(r_list)):
            if r_list[j].find("x") == 0:
                print(r_list[j], "0")
            else:
                print(r_list[j], c_array[j])
    np.savez('/tmp/Results', array = array,r_list = r_list,t_list = t_list)

